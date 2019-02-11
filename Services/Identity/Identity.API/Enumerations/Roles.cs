namespace Identity.API.Enumerations
{
    public class Role : Enumeration<Role>
    {
        public static Role ProjectOwner = new Role(1, "Project-Owner");
        public static Role ProjectModerator = new Role(2, "Project-Moderator");
        public static Role ProjectCollaborator = new Role(3, "Project-Collaborator");

        protected Role()
        {

        }

        public Role(int id, string name)
            : base(id, name)
        {

        }
    }
}