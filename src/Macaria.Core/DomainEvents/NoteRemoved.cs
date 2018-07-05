using Macaria.Core.Common;
using Macaria.Core.Models;

namespace Macaria.Core.DomainEvents
{
    public class NoteRemoved: DomainEvent<Note>
    {
        public NoteRemoved(Note note)
        {
            Payload = note;
            EventType = EventTypes.Notes.NoteRemoved;
        }
    }
}
