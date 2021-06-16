using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TagAC.MessageBus;

namespace TagAC.Management.Domain.Events.EventHandlers
{
    public class AccessRevokedEventHandler : INotificationHandler<AccessRevokedEvent>
    {
        private readonly IMessageBus _bus;
        public AccessRevokedEventHandler(IMessageBus messageBus)
        {
            _bus = messageBus;
        }

        public async Task Handle(AccessRevokedEvent notification, CancellationToken cancellationToken)
        {
            await _bus.PublishAsync<AccessRevokedEvent>(notification, "access.revoked", cancellationToken);
        }
    }
}
