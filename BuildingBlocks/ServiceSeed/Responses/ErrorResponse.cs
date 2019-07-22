using System.Collections.Generic;

namespace ServiceSeed.Responses
{
    internal sealed class ErrorResponse
    {
        public bool Success => false;
        public int StatusCode { get; }
        public IEnumerable<string> Errors { get; }

        public ErrorResponse(IEnumerable<string> responseErrors, int statusCode)
        {
            Errors = responseErrors;
            StatusCode = statusCode;
        }

        public ErrorResponse(string responseError, int statusCode)
        {
            Errors = new List<string>{ responseError };
            StatusCode = statusCode;
        }
    }
}