using System;
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
    }
}
