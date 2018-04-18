using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Macaria.API.Features.Tenants
{
    [Authorize]
    [ApiController]
    [Route("api/tenants")]
    public class TenantsController
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantsController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpPost("{tenantId}/verify")]
        public async Task Verify([FromRoute]VerifyTenantCommand.Request request)
            => await _mediator.Send(request);

        [HttpPost]
        public async Task<ActionResult<SaveTenantCommand.Response>> Save(SaveTenantCommand.Request request)
            => await _mediator.Send(request);

        [HttpDelete("{tenantId}")]
        public async Task Remove([FromRoute]RemoveTenantCommand.Request request)
            => await _mediator.Send(request);

        [HttpGet("{tenantId}")]
        public async Task<ActionResult<GetTenantByIdQuery.Response>> GetById([FromRoute]GetTenantByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetTenantsQuery.Response>> Get()
            => await _mediator.Send(new GetTenantsQuery.Request());
    }
}
