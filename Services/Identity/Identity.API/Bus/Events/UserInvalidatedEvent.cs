using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Identity.API.Bus.Events
{
    public sealed class UserInvalidatedEvent
    {
        public IEnumerable<string> Errors { get; set; }

        public UserInvalidatedEvent(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}