namespace ServiceSeed.Responses
{
    public class SuccessResponse
    {
        public bool Success => true;
        public object Content { get; }

        public SuccessResponse(object content)
        {
            Content = content;
        }
    }
}
