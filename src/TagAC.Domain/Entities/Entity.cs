using TagAC.Domain.Interfaces;

namespace TagAC.Domain.Entities
{
    public abstract class Entity<TId> : IAggregationRoot
    {
        public TId Id { get; set; }        
    }
}
