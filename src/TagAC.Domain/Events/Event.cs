using MediatR;
using System;
using TagAC.Domain.Messages;

namespace TagAC.Domain.Events
{
    public class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
