using FluentValidation;
using Macaria.Core.Models;
using Macaria.Core.Extensions;
using Macaria.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.API.Features.Notes
{
    public class SaveNoteCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Note.NoteId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public NoteApiModel Note { get; set; }
        }

        public class Response
        {			
            public int NoteId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var note = await _context.Notes
                    .Include(x => x.NoteTags)
                    .Include("NoteTags.Tag")
                    .SingleOrDefaultAsync(x => request.Note.NoteId == x.NoteId);

                if (note == null) _context.Notes.Add(note = new Note());

                note.Body = request.Note.Body;

                note.Title = request.Note.Title;

                note.Slug = request.Note.Title.ToSlug();
                
                note.NoteTags.Clear();

                foreach(var tag in request.Note.Tags)
                {
                    note.NoteTags.Add(new NoteTag()
                    {
                        Tag = (await _context.Tags.FindAsync(tag.TagId))
                    });
                }

                note.RaiseDomainEvent(new NoteSavedEvent.DomainEvent(note));

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { NoteId = note.NoteId };
            }
        }
    }
}
