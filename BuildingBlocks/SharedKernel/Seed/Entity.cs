using System;

namespace SharedKernel.Seed
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public override bool Equals(object anotherObject)
        {
            var compareTo = anotherObject as Entity;

            if (ReferenceEquals(this, compareTo)) return true;

            var anotherNull = compareTo is null;
            return !anotherNull && Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity x, Entity y)
        {
            if (x is null && y is null)
                return true;

            if (x is null || y is null)
                return false;

            return x.Equals(y);
        }

        public static bool operator !=(Entity x, Entity y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [{Id}]";
        }
    }
}
