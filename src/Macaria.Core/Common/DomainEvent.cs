using MediatR;
namespace Macaria.Core.Common
{
    public class DomainEvent<T>: INotification
    {
        public string EventType { get; set; }
        public T Payload { get; set; }
    }
}
