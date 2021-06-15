using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Apis.AccessControl.Helpers;
using TagAC.Apis.AccessControl.Repositories;
using TagAC.Apis.AccessControl.Sessions;
using TagAC.Domain.Enums;
using TagAC.Management.Domain.Events;
using TagAC.Management.Domain.Events.RequestEvents;
using TagAC.MessageBus;

namespace TagAC.Apis.AccessControl.Services
{
    public class AccessControlService : IAccessControlService
    {
        private readonly IHeaderParametersSession _headerParams;
        private readonly ICacheRepository _cache;
        private readonly ILogger<AccessControlService> _logger;
        private readonly IMessageBus _messageBus;

        private static string Authorized => ((int)AuthorizationStatus.Authorized).ToString();
        private static string Unauthorized => ((int)AuthorizationStatus.Unauthorized).ToString();


        public AccessControlService(IHeaderParametersSession session
            , ICacheRepository cache
            , ILogger<AccessControlService> logger,
            IMessageBus messageBus)
        {
            _headerParams = session;
            _cache = cache;
            _logger = logger;
            _messageBus = messageBus;
        }

        public async Task<AuthorizationStatus> GetAuthorization(CancellationToken cancellationToken)
        {
            var cachedValue = await _cache.GetValue(_headerParams.ToCacheKey(), cancellationToken);
            if (string.IsNullOrWhiteSpace(cachedValue))
            {
                _logger.LogInformation("Authorization not cached. Requesting from management api.");

                var statusResponse = await _messageBus.RequestAsync<AuthorizationStatusRequest, AuthorizationStatusResponse>(
                    new AuthorizationStatusRequest() { RFID = _headerParams.RFID, SmartLockId = _headerParams.SmartLockId }, cancellationToken);

                if (statusResponse != null && statusResponse.Status.HasValue)
                {
                    if (statusResponse.Status == AuthorizationStatus.Authorized)
                    {
                        await GrantAccess(_headerParams.RFID, _headerParams.SmartLockId, cancellationToken);
                    }
                    else
                    {
                        await RevokeAccess(_headerParams.RFID, _headerParams.SmartLockId, cancellationToken);
                    }

                    return statusResponse.Status.Value;
                }
            }

            if (cachedValue == Authorized)
            {
                await PublishAuthorized(cancellationToken);
                return AuthorizationStatus.Authorized;
            }
            
            await PublishDenied(cancellationToken);
            return AuthorizationStatus.Unauthorized;
        }

        private async Task PublishAuthorized(CancellationToken cancellationToken)
        {
            await _messageBus.PublishAsync<AccessAuthorizedRequest>(new AccessAuthorizedRequest()
            {
                RFID = _headerParams.RFID,
                SmartLockId = _headerParams.ToSmartLockId(),
                Date = DateTime.UtcNow
            }, "access.authorized", cancellationToken);
        }
        
        private async Task PublishDenied(CancellationToken cancellationToken)
        {
            await _messageBus.PublishAsync<AccessDeniedRequest>(new AccessDeniedRequest()
            {
                RFID = _headerParams.RFID,
                SmartLockId = _headerParams.ToSmartLockId(),
                Date = DateTime.UtcNow
            }, "access.denied", cancellationToken);
        }

        public async Task GrantAccess(string rfId, string deviceId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Granting access to {deviceId} for {rfId}.");

            await _cache.SetValue(CacheHelper.ToCacheKey(rfId, deviceId), Authorized, cancellationToken);
        }

        public async Task RevokeAccess(string rfId, string deviceId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Revoking access to {deviceId} for {rfId}.");

            await _cache.SetValue(CacheHelper.ToCacheKey(rfId, deviceId), Unauthorized, cancellationToken);
        }
    }
}
