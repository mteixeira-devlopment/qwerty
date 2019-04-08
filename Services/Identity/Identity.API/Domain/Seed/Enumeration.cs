using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Identity.API.Domain.Seed
{
    public abstract class Enumeration<TEnum> where TEnum : Enumeration<TEnum>
    {
        public int Id { get; }
        public string Name { get; }

        protected Enumeration()
        { }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static IEnumerable<TEnum> GetAll()
        {
            var fields = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return fields.Select(f => f.GetValue(null)).Cast<TEnum>();
        }
        
        public static TEnum From(int id)
        {
            return Get(item => item.Id == id);
        }

        public static TEnum FromName(string name)
        {
            return Get(item => item.Name == name);
        }

        protected static TEnum Get(Func<TEnum, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public override int GetHashCode() => Id.GetHashCode();
        public override string ToString() => Name;
    }
}