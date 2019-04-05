using System;

// ReSharper disable once CheckNamespace
namespace Bus.Events
{
    public sealed class AccountValidatedEvent
    {
        public Guid UserId { get; set; }
    }
}