namespace SharedKernel.Responses
{
    public sealed class CommandResponse
    {
        public bool Success { get; }
        public object Content { get; set; }

        public CommandResponse(bool success, object content = null)
        {
            Success = success;
            Content = content;
        }
    }
}