using Macaria.Core.Common;
using System;

namespace Macaria.Core.DomainEvents
{
    public class NoteRemoved: DomainEvent
    {
        public NoteRemoved(Guid noteId)
        {
            NoteId = noteId;
        }
        public Guid NoteId { get; set; }
    }
}
