using Macaria.API.Features.Tags;
using Macaria.Core.ApiModels;
using Macaria.Core.DomainEvents;
using Macaria.Core.Models;
using Macaria.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
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
                    Tag = new TagApiModel()
                    {
                        Name = "Quinntyne"
                    }
                }, default(CancellationToken));

                Assert.Equal(1, response.TagId);
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
                context.Tags.Add(new Tag()
                {
                    TagId = 1,
                    Name = "Quinntyne",
                    
                });

                context.SaveChanges();

                var handler = new GetTagByIdQuery.Handler(context);

                var response = await handler.Handle(new GetTagByIdQuery.Request()
                {
                    TagId = 1
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
                context.Notes.Add(new Note()
                {
                    NoteId = 1,
                    Title = "Angular",
                    Slug = "angular"
                });

                context.Tags.Add(new Tag()
                {
                    TagId = 1,
                    Name = "Routing",
                    Slug = "routing",
                    NoteTags = new List<NoteTag>()
                    {
                        new NoteTag() { NoteId = 1 }
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
                    TagId = 1,
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
                context.Tags.Add(new Tag()
                {
                    TagId = 1,
                    Name = "Quinntyne",
                    
                });

                context.SaveChanges();

                var handler = new RemoveTagCommand.Handler(context);

                await handler.Handle(new RemoveTagCommand.Request()
                {
                    TagId =  1 
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
                context.Tags.Add(new Tag()
                {
                    TagId = 1,
                    Name = "Quinntyne",
                    
                });

                context.SaveChanges();

                var handler = new SaveTagCommand.Handler(context);

                var response = await handler.Handle(new SaveTagCommand.Request()
                {
                    Tag = new TagApiModel()
                    {
                        TagId = 1,
                        Name = "Quinntyne"
                    }
                }, default(CancellationToken));

                Assert.Equal(1, response.TagId);
                Assert.Equal("Quinntyne", context.Tags.Single(x => x.TagId == 1).Name);
            }
        }
    }
}
