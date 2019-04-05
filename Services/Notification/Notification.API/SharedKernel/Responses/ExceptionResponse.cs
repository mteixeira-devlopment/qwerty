namespace Notification.API.SharedKernel.Responses
{
    public class ExceptionResponse
    {
        public bool Success => false;
        public int ResponseStatusCode { get; }
        public string Message { get; }
        public string StackTrace { get; }

        public ExceptionResponse(int responseStatusCode, string message, string stackTrace)
        {
            ResponseStatusCode = responseStatusCode;
            Message = message;
            StackTrace = stackTrace;
        }
    }
}