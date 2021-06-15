using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TagAC.Domain.Interfaces;
using TagAC.Management.Domain.Entities;

namespace TagAC.Management.Domain.Interfaces
{
    public interface IAccessControlRepository : IRepository<AccessControl, Guid>
    {
        Task<AccessControl> GetCredentials(string rfid, Guid lockId);
        Task<IEnumerable<AccessControl>> ListAll(string RFID);
    }
}
