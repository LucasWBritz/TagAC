using System;
using TagAC.Domain.Events;

namespace TagAC.Management.Domain.Events
{
    public class AccessRevokedEvent : DomainEvent
    {
        public AccessRevokedEvent(Guid smartLockdId, string rfid)
        {
            SmartLockId = smartLockdId;
            RFID = rfid;
        }

        public Guid SmartLockId { get; set; }
        public string RFID { get; set; }
    }
}
