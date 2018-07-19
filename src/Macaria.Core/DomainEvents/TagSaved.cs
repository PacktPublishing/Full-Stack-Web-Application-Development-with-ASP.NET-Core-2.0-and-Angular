using Macaria.Core.Common;
using System;

namespace Macaria.Core.DomainEvents
{
    public class TagSaved: DomainEvent {
        public TagSaved(Guid tagId) => TagId = tagId;
        public Guid TagId { get; set; }
    }
    
}
