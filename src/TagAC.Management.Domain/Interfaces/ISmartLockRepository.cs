using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TagAC.Domain.Interfaces;
using TagAC.Management.Domain.Entities;

namespace TagAC.Management.Domain.Interfaces
{
    public interface ISmartLockRepository: IRepository<SmartLock, Guid>
    {
        Task<bool> LockExists(Guid lockId);
        Task<IEnumerable<SmartLock>> ListAll();
    }
}
