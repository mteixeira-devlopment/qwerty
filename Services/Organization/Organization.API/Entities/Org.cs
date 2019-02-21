using System;

namespace Organization.API.Entities
{
    public class Org : Entity<Org>
    {
        public Guid IdUser { get; private set; }

        public string Name { get; private set; }

        protected Org()
        {

        }

        public Org(Guid idUser, string name)
        {
            IdUser = idUser;
            Name = name;
        }
    }
}