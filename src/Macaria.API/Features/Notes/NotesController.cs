using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Macaria.API.Features.Notes
{
    [Authorize]
    [ApiController]
    [Route("api/notes")]
    public class NotesController
    {
        private readonly IMediator _mediator;

        public NotesController(IMediator mediator) => _mediator = mediator;

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<GetNoteBySlugQuery.Response>> GetBySlug([FromRoute]GetNoteBySlugQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet("deleted")]
        public async Task<ActionResult<GetDeletedNotesQuery.Response>> GetDeleted()
            => await _mediator.Send(new GetDeletedNotesQuery.Request());

        [HttpPost("undelete")]
        public async Task UnDelete(UndoNoteDeleteCommand.Request request)
            => await _mediator.Send(request);

        [HttpPost]
        public async Task<ActionResult<SaveNoteCommand.Response>> Save(SaveNoteCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{noteId}")]
        public async Task Remove([FromRoute]RemoveNoteCommand.Request request)
            => await _mediator.Send(request);

        [HttpGet("{noteId}")]
        public async Task<ActionResult<GetNoteByIdQuery.Response>> GetById([FromRoute]GetNoteByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetNotesQuery.Response>> Get()
            => await _mediator.Send(new GetNotesQuery.Request());
    }
}
