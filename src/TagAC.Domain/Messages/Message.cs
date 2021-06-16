using System;

namespace TagAC.Domain.Messages
{
    public class Message
    {
        public string MessageType { get; protected set; }
        public Guid EntityId { get; set; }

        public Message()
        {
            MessageType = GetType().Name;
        }
    }
}
