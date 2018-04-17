using Macaria.API.Features.Tenants;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestUtilities.Extensions;
using Xunit;

namespace IntegrationTests.Features.Tenants
{
    public class TenantScenarios: TenantScenarioBase
    {

        [Fact]
        public async Task ShouldSave()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveTenantCommand.Request, SaveTenantCommand.Response>(Post.Tenants, new SaveTenantCommand.Request() {
                        Tenant = new TenantApiModel()
                        {
                            Name = "Default"
                        }
                    });

                Assert.True(response.TenantId != default(Guid));
            }
        }

        [Fact]
        public async Task ShouldVerify()
        {
            using (var server = CreateServer())
            {                
                var response = await server.CreateClient()
                    .PostAsync(Post.Verify(new Guid("60DE04D9-E441-E811-9D3A-D481D7227E7A")),null);

                response.EnsureSuccessStatusCode();
                
            }
        }
        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetTenantsQuery.Response>(Get.Tenants);

                Assert.True(response.Tenants.Count() > 0);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetTenantByIdQuery.Response>(Get.TenantById(new Guid("60DE04D9-E441-E811-9D3A-D481D7227E7A")));

                Assert.True(response.Tenant.TenantId != default(Guid));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetTenantByIdQuery.Response>(Get.TenantById(new Guid("60DE04D9-E441-E811-9D3A-D481D7227E7A")));

                Assert.True(getByIdResponse.Tenant.TenantId != default(Guid));

                getByIdResponse.Tenant.Name = "Default";

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveTenantCommand.Request, SaveTenantCommand.Response>(Post.Tenants, new SaveTenantCommand.Request()
                    {
                        Tenant = getByIdResponse.Tenant
                    });

                Assert.True(saveResponse.TenantId != default(Guid));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Tenant(new Guid("60DE04D9-E441-E811-9D3A-D481D7227E7A")));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
