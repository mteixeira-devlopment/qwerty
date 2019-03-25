using System;

// ReSharper disable once CheckNamespace
namespace Bus.Events
{
    public sealed class UserInvalidatedEvent
    {
        public Guid UserId { get; set; }
        public string InvalidateReason { get; set; }

        public UserInvalidatedEvent(string invalidateReason)
        {
            InvalidateReason = invalidateReason;
        }
    }
}