using System;

namespace Organization.API.Entities
{
    public class Organization : Entity<Organization>
    {
        public Guid IdUser { get; set; }

        public string Name { get; set; }
    }
}