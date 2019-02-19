using System;

namespace Gateway.API.Entities
{
    public class Service : Entity<Service>
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Port { get; private set; }

        protected Service()
        {
            
        }

        public Service(string name, string address, string port)
        {
            Name = name;
            Address = address;
            Port = port;
        }
    }

    public class Action : Entity<Action>
    {
        protected Action()
        {

        }

        public int ServiceId { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }

        public Service Service { get; private set; }

        public Action(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }

    public abstract class Entity<TEntity> where TEntity : class
    {
        protected Entity()
        {
            
        }

        public Guid Id { get; set; }
    }
}
