using System.Collections.Generic;

namespace ServiceSeed.Responses
{
    internal sealed class ErrorResponse
    {
        public bool Success => false;
        public int StatusCode { get; }
        public ICollection<string> Errors { get; }

        public ErrorResponse(ICollection<string> responseErrors, int statusCode)
        {
            Errors = responseErrors;
            StatusCode = statusCode;
        }
    }
}