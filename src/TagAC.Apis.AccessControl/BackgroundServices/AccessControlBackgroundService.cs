using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Apis.AccessControl.Services;
using TagAC.Management.Domain.Events;
using TagAC.MessageBus;

namespace TagAC.Apis.AccessControl.BackgroundServices
{
    public class AccessControlBackgroundService : BackgroundService
    {
        private readonly ILogger<AccessControlBackgroundService> _logger;
        private readonly IMessageBus _messageBus;

        private readonly IServiceProvider _serviceProvider;

        public AccessControlBackgroundService(ILogger<AccessControlBackgroundService> logger,
            IMessageBus messageBus,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting AccessControlBackgroundService");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stoping AccessControlBackgroundService");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageBus.SubscribeAsync<AccessGrantedEvent>("access.granted", accessGranted => HandleAccessGranted(accessGranted, stoppingToken), stoppingToken);
            await _messageBus.SubscribeAsync<AccessRevokedEvent>("access.revoked", accessRevoked => HandleAccessRevoked(accessRevoked, stoppingToken), stoppingToken);
        }

        private async Task HandleAccessGranted(AccessGrantedEvent accessGranted, CancellationToken cancellationToken)
        {
            using(var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAccessControlService>();

                await service.GrantAccess(accessGranted.RFID, accessGranted.SmartLockId.ToString(), cancellationToken);
            }
        }

        private async Task HandleAccessRevoked(AccessRevokedEvent accessRevoked, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAccessControlService>();

                await service.RevokeAccess(accessRevoked.RFID, accessRevoked.SmartLockId.ToString(), cancellationToken);
            }            
        }
    }
}
