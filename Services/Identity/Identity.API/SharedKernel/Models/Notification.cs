using System;

namespace Identity.API.SharedKernel.Models
{
    public class Notification
    {
        public Guid Id { get; private set; }

        public string ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }

        public Notification(string errorMessage)
        {
            Id = Guid.NewGuid();

            ErrorCode = "100";
            ErrorMessage = errorMessage;
        }

        public Notification(string statusCode, string errorMessage)
        {
            Id = Guid.NewGuid();

            ErrorCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}