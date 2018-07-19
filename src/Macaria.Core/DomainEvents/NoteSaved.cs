using Macaria.Core.Common;
using System;

namespace Macaria.Core.DomainEvents
{
    public class NoteSaved: DomainEvent
    {
        public NoteSaved(Guid noteId) => NoteId = noteId;

        public Guid NoteId { get; set; }
    }
    
}
