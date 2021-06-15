using System;
using TagAC.Domain.Events;

namespace TagAC.Management.Domain.Events
{
    public class AccessGrantedEvent : DomainEvent
    {
        public AccessGrantedEvent(Guid smartLockdId, string rfid)
        {
            SmartLockId = smartLockdId;
            RFID = rfid;
        }

        public Guid SmartLockId { get; set; }
        public string RFID { get; set; }
    }
}
