using Macaria.Core.Common;
using Macaria.Core.Models;

namespace Macaria.Core.DomainEvents
{
    public class TagRemoved: DomainEvent<Tag>
    {
        public TagRemoved(Tag tag)
        {
            Payload = tag;
            EventType = EventTypes.Tags.TagRemoved;
        }
    }
    
}
