using System.Collections.Generic;

namespace Identity.API.Responses
{
    public class ErrorResponse
    {
        public bool Success => false;
        public int ResponseStatusCode { get; }
        public List<string> Errors;

        public ErrorResponse(List<string> responseErrors, int responseStatusCode)
        {
            Errors = responseErrors;
            ResponseStatusCode = responseStatusCode;
        }
    }
}