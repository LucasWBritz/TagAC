using System;
using TagAC.Domain.Events;

namespace TagAC.Management.Domain.Events.RequestEvents
{
    public class AccessDeniedRequest : Event
    {
        public DateTime Date { get; set; }
        public string RFID { get; set; }
        public Guid SmartLockId { get; set; }
    }
}
