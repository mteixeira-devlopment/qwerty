namespace Identity.API.Responses
{
    public class SuccessResponse
    {
        public bool Success => true;
        public object Body;

        public SuccessResponse(object responseBody)
        {
            Body = responseBody;
        }
    }
}
