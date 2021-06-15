using TagAC.Domain.Enums;
using TagAC.Domain.Messages;

namespace TagAC.Management.Domain.Events.RequestEvents
{
    public class AuthorizationStatusResponse : Message
    {
        public AuthorizationStatus? Status { get; set; }
    }
}
