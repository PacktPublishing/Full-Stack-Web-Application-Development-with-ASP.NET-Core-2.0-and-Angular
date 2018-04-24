using Macaria.API.Behaviors;
using Macaria.API.Features.Notes;
using Macaria.API.Hubs;
using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.API
{
    public class EntityChangedNotificationBehaviorTests: BaseTestCollection
    {
        private readonly Mock<IHubContext<AppHub>> _hubContextMock;
        
        public EntityChangedNotificationBehaviorTests()
        {
            
        }

        [Fact]
        public async Task ShouldSendNotificationAfterSaveNoteCommand()
        {
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldSendNotificationAfterSaveNoteCommand")
                .Options;

            using (var context = new MacariaContext(options, _httpContextAccessorMock.Object))
            {

                var mockClients = new Mock<IHubClients>();

                var mockGroups = new Mock<IClientProxy>();

                var mockContext = new Mock<IHubContext<AppHub>>();

                mockGroups.Setup((x) => x.SendAsync("message", new JObject())).Verifiable();

                mockClients.Setup(x => x.Group($"{new Guid("60DE04D9-E441-E811-9D3A-D481D7227E7A")}".ToLower())).Returns(mockGroups.Object);

                mockContext.Setup(x => x.Clients).Returns(mockClients.Object);

                var tenant = InsertTenantIntoInMemoryDatabase(context);

                var subject =
                new EntityChangedNotificationBehavior<SaveNoteCommand.Request, SaveNoteCommand.Response>
                (mockContext.Object, context, _httpContextAccessorMock.Object);
                
                var response = await subject.Handle(new SaveNoteCommand.Request()
                {
                    Note = new NoteApiModel()
                    {
                        NoteId = 1,
                        Title = "Quinntyne"
                    }
                }, default(CancellationToken), () => {

                    context.Notes.Add(new Note()
                    {
                        NoteId = 1,
                        Tenant = tenant
                    });

                    context.SaveChanges();

                    return Task.FromResult(new SaveNoteCommand.Response()
                    {
                        NoteId = 1
                    });                    
                });
                Assert.NotNull(response);
            }
        }

        [Fact]
        public async Task ShouldSendNotificationAfterRemoveNoteCommand()
        {

        }

        [Fact]
        public async Task ShouldSendNotificationAfterSaveTagCommand()
        {

        }

        [Fact]
        public async Task ShouldSendNotificationAfterRemoveTagCommand()
        {

        }
    }
}
