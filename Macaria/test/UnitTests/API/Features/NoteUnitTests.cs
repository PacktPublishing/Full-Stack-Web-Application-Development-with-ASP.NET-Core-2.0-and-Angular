using Macaria.API.Features.Notes;
using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.API.Features
{
    public class NoteUnitTests : BaseFeatureTestCollection
    {     
 
        [Fact]
        public async Task ShouldHandleSaveNoteCommandRequest()
        {

            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleSaveNoteCommandRequest")
                .Options;

            using (var context = new MacariaContext(options, _httpContextAccessorMock.Object))
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

            using (var context = new MacariaContext(options, _httpContextAccessorMock.Object))
            {
                var tenant = InsertTenantIntoInMemoryDatabase(context);

                context.Notes.Add(new Note()
                {
                    NoteId = 1,
                    Title = "Quinntyne",
                    Tenant = tenant
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

            using (var context = new MacariaContext(options, _httpContextAccessorMock.Object))
            {
                var tenant = InsertTenantIntoInMemoryDatabase(context);

                context.Notes.Add(new Macaria.Core.Entities.Note()
                {
                    NoteId = 1,
                    Title = "Quinntyne",
                    Tenant = tenant
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

            using (var context = new MacariaContext(options, _httpContextAccessorMock.Object))
            {
                var tenant = InsertTenantIntoInMemoryDatabase(context);

                context.Notes.Add(new Note()
                {
                    NoteId = 1,
                    Title = "Quinntyne",
                    Tenant = tenant
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

            using (var context = new MacariaContext(options, _httpContextAccessorMock.Object))
            {
                var tenant = InsertTenantIntoInMemoryDatabase(context);

                context.Notes.Add(new Note()
                {
                    NoteId = 1,
                    Title = "Quinntyne",
                    Tenant = tenant
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
        
    }
}
