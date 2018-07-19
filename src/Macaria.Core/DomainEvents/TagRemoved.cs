using Macaria.Core.Common;
using System;

namespace Macaria.Core.DomainEvents
{
    public class TagRemoved: DomainEvent
    {
        public TagRemoved(Guid tagId) => TagId = tagId;
        public Guid TagId { get; set; }
    }
}
