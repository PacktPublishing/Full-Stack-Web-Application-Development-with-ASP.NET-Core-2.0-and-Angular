using Macaria.API.Features.Notes;
using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.API
{
    public class NoteUnitTests
    {     
 
        [Fact]
        public async Task ShouldHandleSaveNoteCommandRequest()
        {

            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleSaveNoteCommandRequest")
                .Options;

            using (var context = new MacariaContext(options))
            {
                var handler = new SaveNoteCommand.Handler(context);

                var response = await handler.Handle(new SaveNoteCommand.Request()
                {
                    Note = new NoteApiModel()
                    {
                        Title = "Quinntyne"
                    }
                }, default(CancellationToken));

                Assert.Equal(1, response.NoteId);
            }
        }

        [Fact]
        public async Task ShouldHandleGetNoteByIdQueryRequest()
        {
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetNoteByIdQueryRequest")
                .Options;

            using (var context = new MacariaContext(options))
            {
                

                context.Notes.Add(new Note()
                {
                    NoteId = 1,
                    Title = "Quinntyne",
                    
                });

                context.SaveChanges();

                var handler = new GetNoteByIdQuery.Handler(context);

                var response = await handler.Handle(new GetNoteByIdQuery.Request()
                {
                    NoteId = 1
                }, default(CancellationToken));

                Assert.Equal("Quinntyne", response.Note.Title);
            }
        }

        [Fact]
        public async Task ShouldHandleGetNotesQueryRequest()
        {
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetNotesQueryRequest")
                .Options;

            using (var context = new MacariaContext(options))
            {
                

                context.Notes.Add(new Macaria.Core.Entities.Note()
                {
                    NoteId = 1,
                    Title = "Quinntyne",
                    
                });

                context.SaveChanges();

                var handler = new GetNotesQuery.Handler(context);

                var response = await handler.Handle(new GetNotesQuery.Request(), default(CancellationToken));

                Assert.Single(response.Notes);
            }
        }

        [Fact]
        public async Task ShouldHandleRemoveNoteCommandRequest()
        {
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleRemoveNoteCommandRequest")
                .Options;

            using (var context = new MacariaContext(options))
            {
                

                context.Notes.Add(new Note()
                {
                    NoteId = 1,
                    Title = "Quinntyne",
                    
                });

                context.SaveChanges();

                var handler = new RemoveNoteCommand.Handler(context);

                await handler.Handle(new RemoveNoteCommand.Request()
                {
                    NoteId =  1 
                }, default(CancellationToken));

                Assert.Equal(0, context.Notes.Count());
            }
        }

        [Fact]
        public async Task ShouldHandleUpdateNoteCommandRequest()
        {
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleUpdateNoteCommandRequest")
                .Options;

            using (var context = new MacariaContext(options))
            {
                

                context.Notes.Add(new Note()
                {
                    NoteId = 1,
                    Title = "Quinntyne",
                    
                });

                context.SaveChanges();

                var handler = new SaveNoteCommand.Handler(context);

                var response = await handler.Handle(new SaveNoteCommand.Request()
                {
                    Note = new NoteApiModel()
                    {
                        NoteId = 1,
                        Title = "Quinntyne"
                    }
                }, default(CancellationToken));

                Assert.Equal(1, response.NoteId);
                Assert.Equal("Quinntyne", context.Notes.Single(x => x.NoteId == 1).Title);
            }
        }

        [Fact]
        public async Task ShouldHandleSaveNoteTagCommandRequest()
        {

            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleSaveNoteTagCommandRequest")
                .Options;

            using (var context = new MacariaContext(options))
            {

                

                var note = new Note()
                {
                    NoteId = 1,
                    Title = "My Note",
                    
                };

                context.Notes.Add(note);

                context.Tags.Add(new Tag()
                {
                    TagId = 1,
                    Name = "Angular",
                    
                });

                context.SaveChanges();

                var handler = new AddNoteTagCommand.Handler(context);

                await handler.Handle(new AddNoteTagCommand.Request()
                {
                    NoteId =1,
                    TagId = 1
                }, default(CancellationToken));

                Assert.Single(note.NoteTags);
            }
        }

        [Fact]
        public async Task ShouldHandleRemoveNoteTagCommandRequest()
        {

            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleRemoveNoteTagCommandRequest")
                .Options;

            using (var context = new MacariaContext(options))
            {

                

                var note = new Note()
                {
                    NoteId = 1,
                    Title = "My Note",
                    
                };

                context.Notes.Add(note);
                
                context.Tags.Add(new Tag()
                {
                    TagId = 1,
                    Name = "Angular",
                    
                });

                context.SaveChanges();

                note.NoteTags.Add(new NoteTag()
                {
                    TagId = 1,
                    NoteId = 1,
                    
                });

                context.SaveChanges();

                var handler = new RemoveNoteTagCommand.Handler(context);

                await handler.Handle(new RemoveNoteTagCommand.Request()
                {
                    NoteId = 1,
                    TagId = 1
                }, default(CancellationToken));

                Assert.Empty(note.NoteTags);
            }
        }
    }
}
