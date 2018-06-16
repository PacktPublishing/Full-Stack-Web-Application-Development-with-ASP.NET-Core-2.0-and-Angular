using MediatR;
using System;
using System.Collections.Generic;

namespace Macaria.Core.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {

        }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        private List<INotification> _domainEvents = new List<INotification>();
        public IReadOnlyCollection<INotification> DomainEvents 
            => _domainEvents.AsReadOnly();
        public void RaiseDomainEvent(INotification eventItem) 
            => _domainEvents.Add(eventItem);
        public void ClearEvents() => _domainEvents.Clear();
    }
}
