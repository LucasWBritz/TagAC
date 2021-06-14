using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TagAC.Management.Data.EFCore.Context;
using TagAC.Management.Domain.Entities;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Management.Data.EFCore.Repositories.Entities
{
    public class AccessCredentialsRepository : Repository<AccessCredential, Guid>, IAccessCredentialsRepository
    {
        public AccessCredentialsRepository(ManagementDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<AccessCredential> GetCredentials(string rfid, Guid lockId)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.RFID == rfid && x.SmartLockId == lockId);
        }
    } 
}
