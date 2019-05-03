using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Identity.API.Bus.Events
{
    public sealed class AccountInvalidatedEvent
    {
        public Guid UserId { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}