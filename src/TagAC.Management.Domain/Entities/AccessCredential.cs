using System;
using TagAC.Domain.Entities;
using TagAC.Domain.Enums;

namespace TagAC.Management.Domain.Entities
{
    public class AccessCredential : Entity<Guid>
    {
        public AccessCredential()
        {
            Id = Guid.NewGuid();
        }

        public string UserId { get; set; } // Identity user id.
        public Guid SmartLockId { get; set; }
        public virtual SmartLock SmartLock { get; set; }
        public AuthorizationStatus Status { get; set; }
    }
}
