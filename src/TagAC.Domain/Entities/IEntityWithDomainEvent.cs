using System;
using System.Collections.Generic;
using TagAC.Domain.Events;

namespace TagAC.Domain.Entities
{
    public interface IEntityWithDomainEvent
    {
        Guid Id { get; set; }

        IReadOnlyCollection<Event> Events { get; }
        void AddEvent(Event e);
        void RemoveEvent(Event e);
        void ClearEvents();
    }
}
