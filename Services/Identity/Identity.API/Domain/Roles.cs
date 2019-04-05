using Identity.API.Domain.Seed;

namespace Identity.API.Domain
{
    public class Roles : Enumeration<Roles>
    {
        public static Roles ProjectOwner = new Roles(1, "Project-Owner");
        public static Roles ProjectModerator = new Roles(2, "Project-Moderator");
        public static Roles ProjectCollaborator = new Roles(3, "Project-Collaborator");

        protected Roles()
        {

        }

        public Roles(int id, string name)
            : base(id, name)
        {

        }
    }
}