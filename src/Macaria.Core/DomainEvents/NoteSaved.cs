using Macaria.Core.ApiModels;
using Macaria.Core.Common;
using Macaria.Core.Models;

namespace Macaria.Core.DomainEvents
{
    public class NoteSaved: DomainEvent<NoteApiModel>
    {
        public NoteSaved(Note note)
        {
            Payload =  NoteApiModel.FromNote(note);
            EventType = EventTypes.Notes.NoteSaved;
        }
    }
    
}
