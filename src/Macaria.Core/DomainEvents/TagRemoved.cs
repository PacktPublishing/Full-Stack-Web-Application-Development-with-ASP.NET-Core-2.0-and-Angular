using Macaria.Core.ApiModels;
using Macaria.Core.Common;
using Macaria.Core.Models;

namespace Macaria.Core.DomainEvents
{
    public class TagRemoved: DomainEvent<TagApiModel>
    {
        public TagRemoved(Tag tag)
        {
            Payload = TagApiModel.FromTag(tag);
            EventType = EventTypes.Tags.TagRemoved;
        }
    }
    
}
