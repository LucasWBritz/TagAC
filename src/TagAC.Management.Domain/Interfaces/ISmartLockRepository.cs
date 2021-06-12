using System;
using TagAC.Domain.Interfaces;
using TagAC.Management.Domain.Entities;

namespace TagAC.Management.Domain.Interfaces
{
    public interface ISmartLockRepository: IRepository<SmartLock, Guid>
    {
    }
}
