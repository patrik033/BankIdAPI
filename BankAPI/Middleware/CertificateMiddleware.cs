using Contracts.Implementations;

namespace BankAPI.Middleware
{
    public class CertificateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _certFilePath;

        public CertificateMiddleware(RequestDelegate next,string certFilePath)
        {
            _next = next;
            _certFilePath = certFilePath;
        }

        public async Task Invoke(HttpContext context)
        {
            //lägg till certifikatet
            var certificiateHandler = new CertificateHandler(_certFilePath);
            certificiateHandler.AddCertificateToStores();

            //fortsätt genom pipelinen
            await _next(context);
        }
    }
}
