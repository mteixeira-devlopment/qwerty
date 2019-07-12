namespace ServiceSeed.Responses
{
    public class ExceptionResponse
    {
        public bool Success => false;

        public int StatusCode { get; }
        public string Message { get; }
        public string StackTrace { get; }

        public ExceptionResponse(int statusCode, string message, string stackTrace)
        {
            StatusCode = statusCode;
            Message = message;
            StackTrace = stackTrace;
        }
    }
}