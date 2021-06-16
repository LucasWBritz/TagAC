using System;
using TagAC.Domain.Events;

namespace TagAC.Management.Domain.Events.RequestEvents
{
    public class AuthorizationStatusRequest : Event
    {
        public string RFID { get; set; }
        public string SmartLockId { get; set; }
    }
}
