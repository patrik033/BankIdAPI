namespace Entities.Models.Response
{
    public class CollectResponse
    {
        public string orderRef { get; set; }
        public string status { get; set; }
        public CompletionData completionData { get; set; }
    }
}
