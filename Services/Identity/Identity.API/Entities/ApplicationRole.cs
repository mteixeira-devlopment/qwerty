namespace Identity.API.Entities
{
    public sealed class ApplicationRole : Entity<ApplicationRole>
    {
        public string Name { get; private set; }
    }
}