using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Domain.Enums;
using TagAC.Management.Domain.Entities;
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
            await SetSubscribers(stoppingToken); // to save logs for authorized or denied.
            await SetResponder(stoppingToken); // To respond to calls from the accesscontrol api.
        }

        private async Task SetSubscribers(CancellationToken cancellationToken)
        {
            await _messageBus.SubscribeAsync<AccessAuthorizedRequest>("access.authorized", (authorized) => HandleAccessAuthorized(authorized, cancellationToken), cancellationToken);
            await _messageBus.SubscribeAsync<AccessDeniedRequest>("access.denied", (denied) => HandleAccessDenied(denied, cancellationToken), cancellationToken);
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

        private async Task HandleAccessAuthorized(AccessAuthorizedRequest accessAuthorized, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // Since I'm only storing logs I'm saving them here directly.
                var repository = scope.ServiceProvider.GetRequiredService<IAccessControlHistoryRepository>();
                await repository.CreateAsync(new AccessControlHistory()
                {
                    RFID = accessAuthorized.RFID,
                    SmartLockId = accessAuthorized.SmartLockId,
                    Status = AuthorizationStatus.Authorized,
                    Time = accessAuthorized.Date
                });

                await repository.UnitOfWork.CommitAsync();
            }
        }
        
        private async Task HandleAccessDenied(AccessDeniedRequest accessDenied, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // Since I'm only storing logs I'm saving them here directly.
                var repository = scope.ServiceProvider.GetRequiredService<IAccessControlHistoryRepository>();
                await repository.CreateAsync(new AccessControlHistory()
                {
                    RFID = accessDenied.RFID,
                    SmartLockId = accessDenied.SmartLockId,
                    Status = AuthorizationStatus.Unauthorized,
                    Time = accessDenied.Date
                });

                await repository.UnitOfWork.CommitAsync();
            }
        }
    }
}
