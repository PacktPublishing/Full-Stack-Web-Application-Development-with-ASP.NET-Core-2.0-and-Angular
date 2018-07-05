using MediatR;
using System;

namespace Macaria.Core.Common
{
    public class DomainEvent<T>: INotification
    {
        public string EventType { get; set; }
        public T Payload { get; set; }
    }
}
