using SharedKernel.Seed;

namespace Notification.API.Domain
{
    public sealed class Notification : Entity
    {
        public string Title { get; private set; }
        public string Summary { get; private set; }

        private Notification()
        {
            
        }

        public Notification(string title, string summary)
        {
            Title = title;
            Summary = summary;
        }
    }
}