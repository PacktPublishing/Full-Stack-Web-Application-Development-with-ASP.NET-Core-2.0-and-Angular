using Macaria.Core.ApiModels;
using Macaria.Core.Common;
using Macaria.Core.Models;

namespace Macaria.Core.DomainEvents
{
    public class TagSaved: DomainEvent<TagApiModel>
    {
        public TagSaved(Tag tag)
        {
            Payload = TagApiModel.FromTag(tag);
            EventType = EventTypes.Tags.TagSaved;
        }
    }
}
