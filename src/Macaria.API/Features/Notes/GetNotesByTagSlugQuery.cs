using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Macaria.Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Macaria.API.Features.Notes
{
    public class GetNotesByTagSlugQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.Slug).NotEqual(default(string));
            }
        }

        public class Request : IRequest<Response> {

            public string Slug { get; set; }
        }

        public class Response
        {
            public IEnumerable<NoteApiModel> Notes { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IMacariaContext _context { get; set; }
            public Handler(IMacariaContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Notes = await _context.Notes
                        .Include(x => x.NoteTags)
                        .Include("NoteTags.Tag")
                        .Where(x => x.NoteTags.Any(n => n.Tag.Slug == request.Slug))
                        .Select(x => NoteApiModel.FromNote(x))
                        .ToListAsync()
                };
        }
    }
}
