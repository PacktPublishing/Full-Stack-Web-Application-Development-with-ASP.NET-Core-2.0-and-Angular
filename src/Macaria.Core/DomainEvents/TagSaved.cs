using Macaria.Core.Common;
using Macaria.Core.Models;

namespace Macaria.Core.DomainEvents
{
    public class TagSaved: DomainEvent<Tag>
    {
        public TagSaved(Tag tag)
        {
            Payload = tag;
            EventType = EventTypes.Tags.TagSaved;
        }
    }
}
