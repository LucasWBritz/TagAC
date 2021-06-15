using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            return await GetAll().AsNoTracking().AnyAsync(x => x.Id == lockId);
        }

        public async Task<IEnumerable<SmartLock>> ListAll()
        {
            return await GetAll().AsNoTracking().ToListAsync();
        }
    }
}
