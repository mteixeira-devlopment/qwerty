using System;

// ReSharper disable once CheckNamespace
namespace Identity.API.Bus.Events
{
    public sealed class AccountValidatedEvent
    {
        public Guid UserId { get; set; }
    }
}