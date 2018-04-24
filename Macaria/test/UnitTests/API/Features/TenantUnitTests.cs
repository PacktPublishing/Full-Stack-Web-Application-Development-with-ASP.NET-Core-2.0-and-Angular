using Macaria.API.Features.Tenants;
using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.API.Features
{
    public class TenantUnitTests : BaseFeatureTestCollection
    {     
 
        [Fact]
        public async Task ShouldHandleSaveTenantCommandRequest()
        {

            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleSaveTenantCommandRequest")
                .Options;

            using (var context = new MacariaContext(options, _httpContextAccessorMock.Object))
            {
                var handler = new SaveTenantCommand.Handler(context);

                var response = await handler.Handle(new SaveTenantCommand.Request()
                {
                    Tenant = new TenantApiModel()
                    {
                        Name = "Quinntyne"
                    }
                }, default(CancellationToken));

                Assert.NotEqual(default(Guid), response.TenantId);
            }
        }

        [Fact]
        public async Task ShouldHandleGetTenantByIdQueryRequest()
        {
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetTenantByIdQueryRequest")
                .Options;

            using (var context = new MacariaContext(options, _httpContextAccessorMock.Object))
            {
                var guid = Guid.NewGuid();

                context.Tenants.Add(new Tenant()
                {
                    TenantId = guid,
                    Name = "Quinntyne",
                });

                context.SaveChanges();

                var handler = new GetTenantByIdQuery.Handler(context);

                var response = await handler.Handle(new GetTenantByIdQuery.Request()
                {
                    TenantId = guid
                }, default(CancellationToken));

                Assert.Equal("Quinntyne", response.Tenant.Name);
            }
        }

        [Fact]
        public async Task ShouldHandleGetTenantsQueryRequest()
        {
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleGetTenantsQueryRequest")
                .Options;

            using (var context = new MacariaContext(options, _httpContextAccessorMock.Object))
            {
                context.Tenants.Add(new Macaria.Core.Entities.Tenant()
                {
                    TenantId = Guid.NewGuid(),
                    Name = "Quinntyne",
                });

                context.SaveChanges();

                var handler = new GetTenantsQuery.Handler(context);

                var response = await handler.Handle(new GetTenantsQuery.Request(), default(CancellationToken));

                Assert.Single(response.Tenants);
            }
        }

        [Fact]
        public async Task ShouldHandleRemoveTenantCommandRequest()
        {
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleRemoveTenantCommandRequest")
                .Options;

            using (var context = new MacariaContext(options, _httpContextAccessorMock.Object))
            {
                var guid = Guid.NewGuid();
                context.Tenants.Add(new Tenant()
                {
                    TenantId = guid,
                    Name = "Quinntyne"
                });

                context.SaveChanges();

                var handler = new RemoveTenantCommand.Handler(context);

                await handler.Handle(new RemoveTenantCommand.Request()
                {
                    TenantId =  guid 
                }, default(CancellationToken));
                
                Assert.Equal(0, context.Tenants.Where(x => x.IsDeleted == false).Count());
            }
        }

        [Fact]
        public async Task ShouldHandleUpdateTenantCommandRequest()
        {
            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleUpdateTenantCommandRequest")
                .Options;

            using (var context = new MacariaContext(options, _httpContextAccessorMock.Object))
            {
                var id = Guid.NewGuid();

                context.Tenants.Add(new Tenant()
                {
                    TenantId = id,
                    Name = "Quinntyne",
                });

                context.SaveChanges();

                var handler = new SaveTenantCommand.Handler(context);

                var response = await handler.Handle(new SaveTenantCommand.Request()
                {
                    Tenant = new TenantApiModel()
                    {
                        TenantId = id,
                        Name = "Quinntyne"
                    }
                }, default(CancellationToken));
                
                Assert.Equal("Quinntyne", context.Tenants.Single(x => x.TenantId == id).Name);
            }
        }
        
    }
}
