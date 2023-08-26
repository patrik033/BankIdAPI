namespace BankAPI.Models.User.Response
{
    public class CompletionData
    {
        public string bankIdIssueDate { get; set; }
        public Device device { get; set; }
        public string ocspResponse { get; set; }
        public string signature { get; set; }
        public User user { get; set; }
    }
}
