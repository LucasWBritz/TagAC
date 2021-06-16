using TagAC.Management.Data.EFCore.Context;
using TagAC.Management.Domain.Entities;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Management.Data.EFCore.Repositories.Entities
{
    public class AccessControlHistoryRepository : Repository<AccessControlHistory, long>, IAccessControlHistoryRepository
    {
        public AccessControlHistoryRepository(ManagementDBContext dbContext) : base(dbContext)
        {
        }
    }
}
