using System;

// ReSharper disable once CheckNamespace
namespace Bus.Events
{
    public sealed class AccountInvalidatedEvent
    {
        public Guid UserId { get; set; }
        public string InvalidateReason { get; set; }

        public AccountInvalidatedEvent(Guid userId, string invalidatedReason)
        {
            UserId = userId;
            InvalidateReason = invalidatedReason;
        }
    }
}