using System;

namespace Core.Entities
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; protected set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
