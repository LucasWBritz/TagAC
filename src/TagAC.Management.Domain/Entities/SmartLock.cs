using System;
using TagAC.Domain.Entities;

namespace TagAC.Management.Domain.Entities
{
    public class SmartLock : EntityWithDomainEvents, IEntityWithDomainEvent
    {
        public SmartLock()
        {
            Id = Guid.NewGuid();
        }

        public string Name { get; set; }
    }
}
