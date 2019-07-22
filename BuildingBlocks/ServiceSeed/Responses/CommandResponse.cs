namespace ServiceSeed.Responses
{
    public class CommandResponse
    {
        public int ExecutionResult { get; }
        public object Content { get; private set; }

        public CommandResponse(int executionResult)
            => ExecutionResult = executionResult;

        public CommandResponse(int executionResult, object content)
        {
            ExecutionResult = executionResult;
            Content = content;
        }
    }
}