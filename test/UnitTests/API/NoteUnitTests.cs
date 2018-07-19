using Macaria.API.Features.Notes;
using Macaria.API.Features.Tags;
using Macaria.Core.DomainEvents;
using Macaria.Core.Models;
using Macaria.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
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

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleSaveNoteCommandRequest")
                .Options;

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Publish(It.IsAny<NoteSaved>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            using (var context = new AppDbContext(options, mediator.Object))
            {
                var handler = new SaveNoteCommand.Handler(context);

                var tag = new Tag()
                {
                    Name = "Angular"
                };

                context.Tags.Add(tag);

                context.SaveChanges();

                var response = await handler.Handle(new SaveNoteCommand.Request()
                {
                    Note = new NoteDto()
                    {
                        Title = "Quinntyne",
                        Tags = new List<TagDto>() { TagDto.FromTag(tag) }
                    }
                }, default(CancellationToken));

                Assert.NotEqual(default(Guid), response.NoteId);
            }
        }

        [Fact]
        public async Task ShouldHandleGetNoteByIdQueryRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetNoteByIdQueryRequest")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var note = new Note()
                {
                    Title = "Quinntyne"
                };

                context.Notes.Add(note);

                context.SaveChanges();

                var handler = new GetNoteByIdQuery.Handler(context);

                var response = await handler.Handle(new GetNoteByIdQuery.Request()
                {
                    NoteId = note.NoteId
                }, default(CancellationToken));

                Assert.Equal("Quinntyne", response.Note.Title);
            }
        }

        [Fact]
        public async Task ShouldHandleGetNoteBySlugQueryRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetNoteBySlugQueryRequest")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var tag = new Tag()
                {
                    Name = "Angular",
                    Slug = "angular"
                };

                context.Tags.Add(tag);

                var note = new Note()
                {
                    Title = "Quinntyne",
                    Slug = "quinntyne",
                    NoteTags = new List<NoteTag>() {
                        new NoteTag() { TagId = tag.TagId }
                    }
                };

                context.Notes.Add(note);

                context.SaveChanges();

                var handler = new GetNoteBySlugQuery.Handler(context);

                var response = await handler.Handle(new GetNoteBySlugQuery.Request()
                {
                    Slug = "quinntyne"
                }, default(CancellationToken));

                Assert.Equal("Quinntyne", response.Note.Title);
                Assert.Single(response.Note.Tags);
            }
        }

        [Fact]
        public async Task ShouldHandleGetNotesQueryRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetNotesQueryRequest")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Notes.Add(new Note()
                {
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
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Publish(It.IsAny<NoteRemoved>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleRemoveNoteCommandRequest")
                .Options;

            using (var context = new AppDbContext(options, mediator.Object))
            {
                var note = new Note()
                {
                    Title = "Quinntyne"
                };

                context.Notes.Add(note);

                context.SaveChanges();

                var handler = new RemoveNoteCommand.Handler(context);
                
                await handler.Handle(new RemoveNoteCommand.Request()
                {
                    NoteId = note.NoteId
                }, default(CancellationToken));

                Assert.Equal(0, context.Notes.Count());
            }
        }

        [Fact]
        public async Task ShouldHandleUpdateNoteCommandRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleUpdateNoteCommandRequest")
                .Options;

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Publish(It.IsAny<NoteSaved>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            using (var context = new AppDbContext(options, mediator.Object))
            {
                var note = new Note()
                {
                    Title = "FooBar"
                };

                context.Notes.Add(note);

                context.SaveChanges();

                var handler = new SaveNoteCommand.Handler(context);

                var response = await handler.Handle(new SaveNoteCommand.Request()
                {
                    Note = new NoteDto()
                    {
                        NoteId = note.NoteId,
                        Title = "Quinntyne"
                    }
                }, default(CancellationToken));

                Assert.Equal(note.NoteId, response.NoteId);
                Assert.Equal("Quinntyne", context.Notes.Single(x => x.NoteId == note.NoteId).Title);
            }
        }
    }
}
