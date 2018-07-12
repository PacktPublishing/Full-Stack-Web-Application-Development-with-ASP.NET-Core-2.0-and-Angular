using Macaria.Core.ApiModels;
using Macaria.Core.Common;
using Macaria.Core.Models;

namespace Macaria.Core.DomainEvents
{
    public class NoteRemoved: DomainEvent<NoteApiModel>
    {
        public NoteRemoved(Note note)
        {
            Payload = NoteApiModel.FromNote(note);
            EventType = EventTypes.Notes.NoteRemoved;
        }
    }
}
