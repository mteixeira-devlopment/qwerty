using ServiceSeed.Actors;

namespace Identity.API.Domain
{
    public sealed class Role : Entity
    {
        public string Name { get; private set; }

        public Role(string name)
        {
            Name = name;
        }
    }
}