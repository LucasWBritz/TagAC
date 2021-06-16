using System;
using TagAC.Domain.Commands;

namespace TagAC.Management.Domain.Commands.GrantAccess
{
    public class GrantAccessCommand : Command<CommandResponse>
    {
        public string RFID { get; set; }
        public Guid SmartLockId { get; set; }
    }
}
