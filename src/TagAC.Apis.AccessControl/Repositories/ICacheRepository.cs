using System.Threading;
using System.Threading.Tasks;

namespace TagAC.Apis.AccessControl.Repositories
{
    public interface ICacheRepository
    {
        Task<string> GetValue(string key, CancellationToken cancelationToken);
        Task SetValue(string key, string value, CancellationToken cancellationToken);
    }
}
