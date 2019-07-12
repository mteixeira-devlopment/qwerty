using System.Collections.Generic;
using System.Linq;

namespace ServiceSeed.Actors
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object anotherObj)
        {
            var thisType = GetType();
            var anotherType = anotherObj.GetType();

            if (anotherObj == null || anotherType != thisType)
                return false;

            var another = (ValueObject)anotherObj;

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

            thisValues.Dispose();
            valuesToCompare.Dispose();

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
            return MemberwiseClone() as ValueObject;
        }
    }
}