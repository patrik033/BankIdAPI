namespace Entities.Models.Response
{
    public class AuthResponse
    {
        public string orderRef { get; set; }
        public string autoStartToken { get; set; }
        public string qrStartToken { get; set; }
        public string qrStartSecret { get; set; }
    }
}
