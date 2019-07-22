using System;

namespace ServiceSeed.Models
{
    public abstract class Notification
    {
        public Guid Id { get; private set; }
        public int Severity { get; set; }
       
        public string Message { get; private set; }

        protected Notification(string message)
        {
            Id = Guid.NewGuid();
          
            Message = message;
        }

        public enum NotificationSeverityTypes
        {
            Information = 0,
            Warning = 1,
            Fail = 2,
            NotExpected = 3
        }
    }

    public class InfoNotification : Notification
    {
        public InfoNotification(string message) : base(message)
        {
            Severity = (int) NotificationSeverityTypes.Information;
        }
    }

    public class WarningNotification : Notification
    {
        public WarningNotification(string message) : base(message)
        {
            Severity = (int)NotificationSeverityTypes.Warning;
        }
    }

    public class FailNotification : Notification
    {
        public FailNotification(string message) : base(message)
        {
            Severity = (int) NotificationSeverityTypes.Fail;
        }
    }

    public class NotExpectedNotification : Notification
    {
        public string StackTrace { get; private set; }

        public string[] ExecutionTrace { get; private set; }

        public NotExpectedNotification(string message, string stackTrace) : base(message)
        {
            Severity = (int) NotificationSeverityTypes.NotExpected;
            StackTrace = stackTrace;
        }

        public void AddExecutionTrace(string[] executionTrace) => ExecutionTrace = executionTrace;
        
    }
}