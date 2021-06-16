using System;
using TagAC.Domain.Commands;

namespace TagAC.Management.Domain.Commands.RevokeAccess
{
    public class RevokeAccessCommand : Command<CommandResponse>
    {
        public string RFID { get; set; }
        public Guid SmartLockId { get; set; }
    }
}
