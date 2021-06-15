using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Domain.Enums;
using TagAC.Management.Domain.Events.RequestEvents;
using TagAC.Management.Domain.Interfaces;
using TagAC.MessageBus;

namespace TagAC.Apis.Management.BackgroundServices
{
    public class ManagementBackgroundService : BackgroundService
    {
        private readonly ILogger<ManagementBackgroundService> _logger;
        private readonly IMessageBus _messageBus;

        private readonly IServiceProvider _serviceProvider;

        public ManagementBackgroundService(ILogger<ManagementBackgroundService> logger,
            IMessageBus messageBus,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(ManagementBackgroundService)}");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stoping {nameof(ManagementBackgroundService)}");

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SetResponder(stoppingToken);
        }

        private async Task SetResponder(CancellationToken cancellationToken)
        {
            await _messageBus.RespondAsync<AuthorizationStatusRequest, AuthorizationStatusResponse>(req => RespondToAuthorizationStatus(req), cancellationToken);
        }

        private async Task<AuthorizationStatusResponse> RespondToAuthorizationStatus(AuthorizationStatusRequest request)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IAccessControlRepository>();

                var accessControl = await repository.GetCredentials(request.RFID, Guid.Parse(request.SmartLockId));

                var acStatus = AuthorizationStatus.Unauthorized;
                if(accessControl != null)
                {
                    acStatus = accessControl.Status;
                }

                return new AuthorizationStatusResponse()
                {
                    Status = acStatus,
                    EntityId = accessControl.Id
                };
            }
        }
    }
}
