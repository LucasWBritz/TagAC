using TagAC.Domain.Interfaces;
using TagAC.Management.Domain.Entities;

namespace TagAC.Management.Domain.Interfaces
{
    public interface IAccessControlHistoryRepository : IRepository<AccessControlHistory, long>
    {
    }
}
