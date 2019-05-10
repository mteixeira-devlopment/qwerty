using System;

namespace SharedKernel.Models
{
    public class Notification
    {
        public Guid Id { get; private set; }
        public string ErrorMessage { get; private set; }

        public Notification(string errorMessage)
        {
            Id = Guid.NewGuid();
            ErrorMessage = errorMessage;
        }
    }
}