namespace ServiceSeed.Responses
{
    public class CommandResponse
    {
        public int ExecutionResult { get; }

        public CommandResponse(int executionResult)
            => ExecutionResult = executionResult;
    }

    public sealed class CommandResponse<TResponse> : CommandResponse
    {
        public TResponse Content { get; private set; }

        public CommandResponse(int executionResult, TResponse content) : base(executionResult)
        {
            Content = content;
        }
    }
}