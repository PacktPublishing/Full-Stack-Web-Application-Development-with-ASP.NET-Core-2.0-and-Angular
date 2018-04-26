using Macaria.API.Features.Tags;
using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.API.Tags
{
    public class TagUnitTests
    {     
 
        [Fact]
        public async Task ShouldHandleSaveTagCommandRequest()
        {

            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleSaveTagCommandRequest")
                .Options;

            using (var context = new MacariaContext(options))
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
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetTagByIdQueryRequest")
                .Options;

            using (var context = new MacariaContext(options))
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
        public async Task ShouldHandleGetTagsQueryRequest()
        {
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetTagsQueryRequest")
                .Options;

            using (var context = new MacariaContext(options))
            {
                

                context.Tags.Add(new Macaria.Core.Entities.Tag()
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
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleRemoveTagCommandRequest")
                .Options;

            using (var context = new MacariaContext(options))
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
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleUpdateTagCommandRequest")
                .Options;

            using (var context = new MacariaContext(options))
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
