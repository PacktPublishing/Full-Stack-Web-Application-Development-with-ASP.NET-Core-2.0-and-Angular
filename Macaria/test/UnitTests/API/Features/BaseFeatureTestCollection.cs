using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using Macaria.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;

namespace UnitTests.API.Features
{
    public class BaseFeatureTestCollection
    {
        
        protected readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        protected readonly Mock<HttpContext> _httpContextMock;

        public BaseFeatureTestCollection()
        {            
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextMock = new Mock<HttpContext>();

            _httpContextMock.Setup(x => x.Items).Returns(new Dictionary<object, object>
            {
                { "TenantId", $"{new Guid("60DE04D9-E441-E811-9D3A-D481D7227E7A")}" },
                { "Username", "quinntynebrown@gmail.com" }
            });

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(_httpContextMock.Object);
        }

        protected Tenant InsertTenantIntoInMemoryDatabase(MacariaContext context) {

            var tenant = new Tenant()
            {
                Name = "Default",
                TenantId = new Guid("60DE04D9-E441-E811-9D3A-D481D7227E7A")
            };

            context.Tenants.Add(tenant);

            context.SaveChanges();

            return tenant;
        }
    }
}
