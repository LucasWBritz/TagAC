using System.Threading;
using System.Threading.Tasks;
using TagAC.Domain.Enums;

namespace TagAC.Apis.AccessControl.Services
{
    public interface IAccessControlService
    {
        Task<AuthorizationStatus> GetAuthorization(CancellationToken cancellationToken);
        Task GrantAccess(string rfId, string deviceId, CancellationToken cancellationToken);
        Task RevokeAccess(string rfId, string deviceId, CancellationToken cancellationToken);
    }
}
