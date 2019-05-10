namespace SharedKernel.Responses
{
    public class QueryResponse
    {
        public bool Found { get; }
        public object Content { get; }

        public QueryResponse(bool found, object content = null)
        {
            Found = found;
            Content = content;
        }
    }
}