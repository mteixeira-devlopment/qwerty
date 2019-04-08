using System.Collections.Generic;
using System.Linq;

namespace Account.API.Domain.Seed
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var another = (ValueObject) obj;

            var thisValues = GetAtomicValues().GetEnumerator();
            var valuesToCompare = another.GetAtomicValues().GetEnumerator();
            
            while (thisValues.MoveNext() && valuesToCompare.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(valuesToCompare.Current, null))
                    return false;
                
                if (thisValues.Current != null 
                    && !thisValues.Current.Equals(valuesToCompare.Current))
                    return false;
            }

            return !thisValues.MoveNext() && !valuesToCompare.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public ValueObject GetCopy()
        {
            return this.MemberwiseClone() as ValueObject;
        }
    }
}