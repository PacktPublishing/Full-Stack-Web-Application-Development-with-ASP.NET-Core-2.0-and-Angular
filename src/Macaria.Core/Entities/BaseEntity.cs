using Macaria.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;

namespace Macaria.Core.Entities
{
    public class BaseEntity: ILoggable
    {
        public BaseEntity() => _domainEvents = new List<INotification>();
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();
        public void RaiseDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);
        public void ClearEvents() => _domainEvents.Clear();
    }
}
