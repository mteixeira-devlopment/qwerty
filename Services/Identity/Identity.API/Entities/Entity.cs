using System;

namespace Identity.API.Entities
{
    public abstract class Entity<TEntity> where TEntity : class
    {
        protected Entity()
        {
            
        }

        public Guid Id { get; set; }
    }
}
