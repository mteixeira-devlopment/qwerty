namespace Notification.API.SharedKernel.Responses
{
    public class SuccessResponse
    {
        public bool Success => true;
        public object Content;

        public SuccessResponse(object responseBody)
        {
            Content = responseBody;
        }
    }
}
