using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TagAC.MessageBus;

namespace TagAC.Management.Domain.Events.EventHandlers
{
    public class AccessGrantedEventHandler : INotificationHandler<AccessGrantedEvent>
    {
        private readonly IMessageBus _bus;
        public AccessGrantedEventHandler(IMessageBus messageBus)
        {
            _bus = messageBus;
        }

        public async Task Handle(AccessGrantedEvent notification, CancellationToken cancellationToken)
        {
            await _bus.PublishAsync<AccessGrantedEvent>(notification, "access.granted", cancellationToken);
        }
    }
}
