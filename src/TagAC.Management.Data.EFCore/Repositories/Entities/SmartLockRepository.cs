using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TagAC.Management.Data.EFCore.Context;
using TagAC.Management.Domain.Entities;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Management.Data.EFCore.Repositories.Entities
{
    public class SmartLockRepository : Repository<SmartLock, Guid>, ISmartLockRepository
    {
        public SmartLockRepository(ManagementDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> LockExists(Guid lockId)
        {
            return await GetAll().AnyAsync(x => x.Id == lockId);
        }
    }
}
