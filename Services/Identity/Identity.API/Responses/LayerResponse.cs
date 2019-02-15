using System.Collections.Generic;

namespace Identity.API.Responses
{
    public class LayerResponse
    {
        public bool Succeeded { get; private set; }
        public List<string> ResponseMessages { get; private set; }

        public LayerResponse()
        {
            ResponseMessages = new List<string>();
        }

        public LayerResponse SuccessResponse()
        {
            Succeeded = true;

            return this;
        }

        public LayerResponse FailureResponse(string message)
        {
            ResponseMessages.Add(message);
            Succeeded = false;

            return this;
        }
    }

    public class LayerResponse<TResponse>
    {
        public TResponse Response { get; private set; }

        public bool Succeeded { get; private set; }
        public List<string> ResponseMessages { get; private set; }

        public LayerResponse()
        {
            ResponseMessages = new List<string>();
        }

        public LayerResponse<TResponse> SuccessResponse(TResponse response)
        {
            Response = response;
            Succeeded = true;

            return this;
        }

        public LayerResponse<TResponse> FailureResponse(string message)
        {
            ResponseMessages.Add(message);
            Succeeded = false;

            return this;
        }
    }
}