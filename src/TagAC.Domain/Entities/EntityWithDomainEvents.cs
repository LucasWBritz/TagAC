using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TagAC.Domain.Events;

namespace TagAC.Domain.Entities
{
    public abstract class EntityWithDomainEvents : Entity<Guid>, IEntityWithDomainEvent
    {        
        private List<Event> _events = new List<Event>();
        
        [JsonIgnore]
        public IReadOnlyCollection<Event> Events => _events?.AsReadOnly();

        public void AddEvent(Event e)
        {
            _events.Add(e);
        }

        public void RemoveEvent(Event e)
        {
            _events.Remove(e);
        }

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
