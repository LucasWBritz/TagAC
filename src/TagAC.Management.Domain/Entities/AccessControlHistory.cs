using System;
using TagAC.Domain.Entities;
using TagAC.Domain.Enums;

namespace TagAC.Management.Domain.Entities
{
    public class AccessControlHistory : Entity<long>
    {
        public string RFID { get; set; }
        public Guid SmartLockId { get; set; }
        public virtual SmartLock SmartLock { get; set; }
        public DateTime Time { get; set; }
        public AuthorizationStatus Status { get; set; }
    }
}
