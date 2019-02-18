using System;

namespace Identity.API.Models
{
    public sealed class ApplicationRole
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}