using System;
using System.Text.Json.Serialization;
using TagAC.Domain.Entities;
using TagAC.Domain.Enums;

namespace TagAC.Management.Domain.Entities
{
    public class AccessControl : EntityWithDomainEvents, IEntityWithDomainEvent
    {
        public AccessControl()
        {
            Id = Guid.NewGuid();
        }

        public string RFID { get; set; } // Identity user id.
        public Guid SmartLockId { get; set; }

        [JsonIgnore]
        public virtual SmartLock SmartLock { get; set; }
        public AuthorizationStatus Status { get; set; }
    }
}
