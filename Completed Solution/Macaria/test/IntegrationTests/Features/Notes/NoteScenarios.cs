using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Macaria.API.Features.Notes;
using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using TestUtilities;
using TestUtilities.Extensions;


namespace IntegrationTests.Features.Notes
{
    public class NoteScenarios: NoteScenarioBase
    {
        [Fact]
        public async Task ShouldSaveNote()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (var server = CreateServer())
            {
                var hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://integrationtests/hub?token={TokenFactory.Get("quinntynebrown@gmail.com")}")
                .WithMessageHandler((h) => server.CreateHandler())
                .WithTransport(TransportType.LongPolling)
                .Build();

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
                            Body = "<p>Something Important</p>"
                        }
                    });

                Assert.True(response.NoteId == 1);

                await tcs.Task;
            }
        }

        [Fact]
        public async Task ShouldAddTag()
        {

            void setUpData(MacariaContext context)
            {
                var note = new Note()
                {
                    Title = "Title1",
                    Body = "Body",
                    Tenant = context.Tenants.First()
                };
                
                context.Notes.Add(note);

                context.SaveChanges();
            }

            using (var server = CreateServer(setUpData))
            {
                var response = await server.CreateClient()
                    .PostAsAsync<AddTagCommand.Request, Task>(Post.AddTag(1,1), new AddTagCommand.Request()
                    {
                        TagId = 1,
                        NoteId = 1
                    });
            }
        }

        [Fact]
        public async Task ShouldRemoveTag()
        {
            void setUpData(MacariaContext context)
            {
                var note = new Note()
                {
                    Title = "Title1",
                    Body = "Body",
                    Tenant = context.Tenants.First()
                };

                note.NoteTags.Add(new NoteTag()
                {
                    Tag = context.Tags.IgnoreQueryFilters().First()
                });

                context.Notes.Add(note);
                
                context.SaveChanges();
            }

            using (var server = CreateServer(setUpData))
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.RemoveTag(1,1));

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            void setUpData(MacariaContext context)
            {
                context.Notes.Add(new Note()
                {
                    Title = "Title1",
                    Body = "Body",
                    Tenant = context.Tenants.First()
                });

                context.Notes.Add(new Note()
                {
                    Title = "Title2",
                    Body = "Body",
                    Tenant = context.Tenants.First()
                });

                context.Notes.Add(new Note()
                {
                    Title = "Title3",
                    Body = "Body",
                    Tenant = context.Tenants.First()
                });

                context.SaveChanges();
            }

            using (var server = CreateServer(setUpData))
            {
                var response = await server.CreateClient()
                    .GetAsync<GetNotesQuery.Response>(Get.Notes);

                Assert.True(response.Notes.Count() == 3);
            }
        }
        
        [Fact]
        public async Task ShouldGetById()
        {
            void setUpData(MacariaContext context)
            {
                context.Notes.Add(new Note()
                {
                    Title = "Title",
                    Body = "Body",
                    Tenant = context.Tenants.First()
                });

                context.SaveChanges();
            }

            using (var server = CreateServer(setUpData))
            {
                var response = await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.NoteById(1));

                Assert.True(response.Note.NoteId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            void setUpData(MacariaContext context)
            {
                context.Notes.Add(new Note()
                {
                    Title = "Title",
                    Body = "Body",
                    Tenant = context.Tenants.First()
                });

                context.SaveChanges();
            }

            using (var server = CreateServer(setUpData))
            {
                
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
        public async Task ShouldDelete()
        {
            void setUpData(MacariaContext context)
            {
                context.Notes.Add(new Note()
                {
                    Title = "Title",
                    Body = "Body",
                    Tenant = context.Tenants.First()
                });

                context.SaveChanges();
            }

            using (var server = CreateServer(setUpData))
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Note(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
