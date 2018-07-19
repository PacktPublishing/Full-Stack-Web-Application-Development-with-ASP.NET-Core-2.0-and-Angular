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
    public class TagUnitTests
    {     
 
        [Fact]
        public async Task ShouldHandleSaveTagCommandRequest()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleSaveTagCommandRequest")
                .Options;

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Publish(It.IsAny<TagSaved>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            using (var context = new AppDbContext(options, mediator.Object))
            {
                var handler = new SaveTagCommand.Handler(context);

                var response = await handler.Handle(new SaveTagCommand.Request()
                {
                    Tag = new TagDto()
                    {
                        Name = "Quinntyne"
                    }
                }, default(CancellationToken));

                Assert.NotEqual(default(Guid),response.TagId);
            }
        }

        [Fact]
        public async Task ShouldHandleGetTagByIdQueryRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetTagByIdQueryRequest")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var tag = new Tag() { Name = "Quinntyne" };

                context.Tags.Add(tag);

                context.SaveChanges();

                var handler = new GetTagByIdQuery.Handler(context);

                var response = await handler.Handle(new GetTagByIdQuery.Request()
                {
                    TagId = tag.TagId
                }, default(CancellationToken));

                Assert.Equal("Quinntyne", response.Tag.Name);
            }
        }

        [Fact]
        public async Task ShouldHandleGetTagBySlugQueryRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetTagBySlugQueryRequest")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var note = new Note()
                {
                    Title = "Angular",
                    Slug = "angular"
                };

                context.Notes.Add(note);

                context.Tags.Add(new Tag()
                {
                    Name = "Routing",
                    Slug = "routing",
                    NoteTags = new List<NoteTag>()
                    {
                        new NoteTag() { NoteId = note.NoteId }
                    }
                });
                
                context.SaveChanges();

                var handler = new GetTagBySlugQuery.Handler(context);

                var response = await handler.Handle(new GetTagBySlugQuery.Request()
                {
                    Slug = "routing"
                }, default(CancellationToken));

                Assert.Equal("angular", response.Tag.Notes.First().Slug);
                Assert.Single(response.Tag.Notes);
            }
        }

        [Fact]
        public async Task ShouldHandleGetTagsQueryRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetTagsQueryRequest")
                .Options;

            using (var context = new AppDbContext(options))
            {                
                context.Tags.Add(new Macaria.Core.Models.Tag()
                {
                    TagId = Guid.NewGuid(),
                    Name = "Quinntyne",
                    
                });

                context.SaveChanges();

                var handler = new GetTagsQuery.Handler(context);

                var response = await handler.Handle(new GetTagsQuery.Request(), default(CancellationToken));

                Assert.Single(response.Tags);
            }
        }

        [Fact]
        public async Task ShouldHandleRemoveTagCommandRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleRemoveTagCommandRequest")
                .Options;

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Publish(It.IsAny<TagRemoved>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            using (var context = new AppDbContext(options, mediator.Object))
            {
                var tag = new Tag()
                {
                    Name = "Quinntyne"
                };

                context.Tags.Add(tag);

                context.SaveChanges();

                var handler = new RemoveTagCommand.Handler(context);

                await handler.Handle(new RemoveTagCommand.Request()
                {
                    TagId = tag.TagId
                }, default(CancellationToken));

                Assert.Equal(0, context.Tags.Count());
            }
        }

        [Fact]
        public async Task ShouldHandleUpdateTagCommandRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleUpdateTagCommandRequest")
                .Options;


            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Publish(It.IsAny<TagSaved>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            using (var context = new AppDbContext(options, mediator.Object))
            {
                var id = Guid.NewGuid();

                context.Tags.Add(new Tag()
                {
                    TagId = id,
                    Name = "Quinntyne",
                    
                });

                context.SaveChanges();

                var handler = new SaveTagCommand.Handler(context);

                var response = await handler.Handle(new SaveTagCommand.Request()
                {
                    Tag = new TagDto()
                    {
                        TagId = id,
                        Name = "Quinntyne"
                    }
                }, default(CancellationToken));

                Assert.Equal(id, response.TagId);
                Assert.Equal("Quinntyne", context.Tags.Single(x => x.TagId == id).Name);
            }
        }
    }
}
