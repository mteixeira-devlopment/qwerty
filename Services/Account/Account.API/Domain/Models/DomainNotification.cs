﻿using System;

namespace Account.API.Domain.Models
{
    public class DomainNotification
    {
        public Guid Id { get; private set; }
        public string ErrorMessage { get; private set; }

        public DomainNotification(string errorMessage)
        {
            Id = Guid.NewGuid();
            ErrorMessage = errorMessage;
        }
    }
}