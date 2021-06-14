using System;
using System.Threading.Tasks;
using TagAC.Domain.Interfaces;
using TagAC.Management.Domain.Entities;

namespace TagAC.Management.Domain.Interfaces
{
    public interface IAccessCredentialsRepository : IRepository<AccessCredential, Guid>
    {
        Task<AccessCredential> GetCredentials(string userId, Guid lockId);
    }
}
