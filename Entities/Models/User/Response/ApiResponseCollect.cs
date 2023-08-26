namespace Entities.Models.User.Response
{
    public class ApiResponseCollect
    {
        public string orderRef { get; set; }
        public string status { get; set; }
        public CompletionData completionData { get; set; }
    }
}
