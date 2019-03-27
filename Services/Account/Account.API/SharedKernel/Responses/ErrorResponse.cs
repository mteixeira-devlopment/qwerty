using System.Collections.Generic;

namespace Account.API.SharedKernel.Responses
{
    internal sealed class ErrorResponse
    {
        public bool Success => false;
        public int ResponseStatusCode { get; }
        public List<string> Errors { get; }

        public ErrorResponse(List<string> responseErrors, int responseStatusCode)
        {
            Errors = responseErrors;
            ResponseStatusCode = responseStatusCode;
        }
    }
}