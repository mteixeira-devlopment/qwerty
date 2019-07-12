namespace ServiceSeed.Responses
{
    public sealed class CommandResponse
    {
        public bool Success { get; }
        public object Content { get; set; }

        public CommandResponse(bool success)
            => Success = success;

        public CommandResponse(bool success, object content)
        {
            Success = success;
            Content = content;
        }
    }
}