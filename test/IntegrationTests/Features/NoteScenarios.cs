using Macaria.API.Features.Notes;
using Macaria.API.Features.Tags;
using Macaria.Core.Extensions;
using Macaria.Core.Models;
using Macaria.Infrastructure.Data;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class NoteScenarios: NoteScenarioBase
    {
        [Fact]
        public async Task ShouldSaveNote()
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (var server = CreateServer())
            {
                var hubConnection = GetHubConnection(server.CreateHandler());

                hubConnection.On<dynamic>("message", (result) =>
                {
                    Assert.Equal("[Note] Saved", $"{result.type}");
                    Assert.Equal(1, Convert.ToInt16(result.payload.note.noteId));
                    tcs.SetResult(true);
                });

                await hubConnection.StartAsync();

                var response = await server.CreateClient()
                    .PostAsAsync<SaveNoteCommand.Request, SaveNoteCommand.Response>(Post.Notes, new SaveNoteCommand.Request() {
                        Note = new NoteApiModel()
                        {
                            Title = "First Note",
                            Body = "<p>Something Important</p>",
                            Tags = new List<TagApiModel>() { new TagApiModel() { TagId = 1, Name = "Angular" } }
                        }
                    });

                Assert.True(response.NoteId == 1);

                await tcs.Task;
            }
        }

        [Fact]
        public async Task ShouldReturnValidationError()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsync(Post.Notes, new SaveNoteCommand.Request()
                    {
                        Note = new NoteApiModel()
                        {
                            Title = "First Note",
                            Body = "",
                            Tags = new List<TagApiModel>() { new TagApiModel() { TagId = 1, Name = "Angular" } }
                        }
                    });
                
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);                
            }
        }

        [Fact]
        public async Task ShouldGetAllNotes()
        {
            using (var server = CreateServer())
            {
                var context = server.Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;

                context.Notes.Add(new Note()
                {
                    Title = "Title1",
                    Body = "Body"
                });

                context.Notes.Add(new Note()
                {
                    Title = "Title2",
                    Body = "Body"
                });

                context.Notes.Add(new Note()
                {
                    Title = "Title3",
                    Body = "Body"
                });

                context.SaveChanges();

                var response = await server.CreateClient()
                    .GetAsync<GetNotesQuery.Response>(Get.Notes);

                Assert.True(response.Notes.Count() == 3);
            }
        }

        [Fact]
        public async Task ShouldGetAllDeletedNotes()
        {
            using (var server = CreateServer())
            {
                var context = server.Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;

                context.Notes.Add(new Note()
                {
                    Title = "Title1",
                    Body = "Body",
                    IsDeleted = true
                });

                context.Notes.Add(new Note()
                {
                    Title = "Title2",
                    Body = "Body"
                });

                context.SaveChanges();

                var response = await server.CreateClient()
                    .GetAsync<GetDeletedNotesQuery.Response>(Get.DeletedNotes);

                Assert.True(response.Notes.Count() == 1);
            }
        }

        [Fact]
        public async Task ShouldUnDeleteNote()
        {
            using (var server = CreateServer())
            {
                var context = server.Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;

                var note = new Note()
                {
                    Title = "Title1",
                    Body = "Body",
                    IsDeleted = true
                };

                context.Notes.Add(note);

                context.SaveChanges();

                await server.CreateClient()
                    .PostAsAsync<UndoNoteDeleteCommand.Request, HttpResponseMessage>(Post.UnDelete,new UndoNoteDeleteCommand.Request() {
                        NoteId = note.NoteId
                    });

                var response = await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.NoteById(note.NoteId));

                Assert.True(response.Note.NoteId == 1);
            }
        }

        [Fact]
        public async Task ShouldGetNoteById()
        {
            using (var server = CreateServer())
            {
                var context = server.Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;

                context.Notes.Add(new Note()
                {
                    Title = "Title",
                    Body = "Body",
                });

                context.SaveChanges();

                var response = await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.NoteById(1));

                Assert.True(response.Note.NoteId != default(int));
            }
        }

        [Fact]
        public async Task ShouldGetNoteBySlug()
        {            
            using (var server = CreateServer())
            {
                var context = server.Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;

                context.Notes.Add(new Note()
                {
                    Title = "Title",
                    Body = "Body",
                    Slug = "title"
                });

                context.SaveChanges();

                var response = await server.CreateClient()
                    .GetAsync<GetNoteBySlugQuery.Response>(Get.NoteBySlug("title"));

                Assert.True(response.Note.NoteId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdateNote()
        {
            using (var server = CreateServer())
            {
                var context = server.Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;

                context.Notes.Add(new Note()
                {
                    Title = "Title",
                    Body = "Body",
                });

                context.SaveChanges();

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveNoteCommand.Request, SaveNoteCommand.Response>(Post.Notes, new SaveNoteCommand.Request()
                    {
                        Note = new NoteApiModel()
                        {
                            NoteId = 1,
                            Title = "Title",
                            Body = "Body"
                        }
                    });

                Assert.True(saveResponse.NoteId == 1);
            }
        }
        
        [Fact]
        public async Task ShouldDeleteNote()
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (var server = CreateServer())
            {
                var context = server.Host.Services.GetService(typeof(AppDbContext)) as AppDbContext;

                context.Notes.Add(new Note()
                {
                    Title = "Title",
                    Body = "Body"
                });

                context.SaveChanges();            

                var hubConnection = GetHubConnection(server.CreateHandler());

                hubConnection.On<dynamic>("message", (result) =>
                {
                    Assert.Equal("[Note] Removed", $"{result.type}");
                    Assert.Equal(1, Convert.ToInt16(result.payload.noteId));
                    tcs.SetResult(true);
                });

                await hubConnection.StartAsync();

                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Note(1));

                response.EnsureSuccessStatusCode();

                await tcs.Task;
            }
        }

    }
}
