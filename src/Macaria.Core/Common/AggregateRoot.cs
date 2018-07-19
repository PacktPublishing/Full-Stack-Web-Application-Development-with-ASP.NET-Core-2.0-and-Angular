using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macaria.Core.Common
{
    public class AggregateRoot
    {
        public AggregateRoot() => _domainEvents = new List<DomainEvent>();
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        private List<DomainEvent> _domainEvents;
        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public void RaiseDomainEvent(DomainEvent eventItem) => _domainEvents.Add(eventItem);
        public void ClearEvents() => _domainEvents.Clear();
    }
}
