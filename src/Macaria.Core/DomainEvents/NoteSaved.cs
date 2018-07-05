using Macaria.Core.Common;
using Macaria.Core.Models;

namespace Macaria.Core.DomainEvents
{
    public class NoteSaved: DomainEvent<Note>
    {
        public NoteSaved(Note note)
        {
            Payload =  note;
            EventType = EventTypes.Notes.NoteSaved;
        }
    }
    
}
