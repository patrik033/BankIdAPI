namespace Entities.Models.Response
{
    public class CompletionData
    {
        public string bankIdIssueDate { get; set; }
        public string ocspResponse { get; set; }
        public string signature { get; set; }
        public UserData user { get; set; }
    }
}
