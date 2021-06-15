using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TagAC.Management.Data.EFCore.Context;
using TagAC.Management.Domain.Entities;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Management.Data.EFCore.Repositories.Entities
{
    public class AccessControlRepository : Repository<AccessControl, Guid>, IAccessControlRepository
    {
        public AccessControlRepository(ManagementDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<AccessControl> GetCredentials(string rfid, Guid lockId)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.RFID == rfid && x.SmartLockId == lockId);
        }

        public async Task<IEnumerable<AccessControl>> ListAll(string RFID)
        {
            return await GetAll().AsNoTracking().Where(x => x.RFID == RFID).ToListAsync();
        }
    } 
}
