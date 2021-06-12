using System;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Apis.AccessControl.Helpers;
using TagAC.Apis.AccessControl.Repositories;
using TagAC.Apis.AccessControl.Sessions;
using TagAC.Core.Enums;

namespace TagAC.Apis.AccessControl.Services
{
    public class AccessControlService : IAccessControlService
    {
        private readonly IHeaderParametersSession _headerParams;
        private readonly ICacheRepository _cache;

        private static string Authorized => ((int)AuthorizationStatus.Authorized).ToString();
        private static string Unauthorized => ((int)AuthorizationStatus.Unauthorized).ToString();


        public AccessControlService(IHeaderParametersSession session, ICacheRepository cache)
        {
            _headerParams = session;
            _cache = cache;
        }

        public async Task<AuthorizationStatus> GetAuthorization(CancellationToken cancellationToken)
        {
            var cachedValue = await _cache.GetValue(_headerParams.ToCacheKey(), cancellationToken);
            if (string.IsNullOrWhiteSpace(cachedValue))
            {
                // TODO: Retrieve this from the api
                await GrantAccess(_headerParams.RFID, _headerParams.DeviceId, cancellationToken);
            }

            if (cachedValue == Authorized)
            {
                return AuthorizationStatus.Authorized;
            }

            return AuthorizationStatus.Unauthorized;
        }

        public async Task GrantAccess(string rfId, string deviceId, CancellationToken cancellationToken)
        {
            await _cache.SetValue(_headerParams.ToCacheKey(), Authorized, cancellationToken);
        }

        public async Task RevokeAccess(string rfId, string deviceId, CancellationToken cancellationToken)
        {
            await _cache.SetValue(_headerParams.ToCacheKey(), Unauthorized, cancellationToken);
        }
    }
}
