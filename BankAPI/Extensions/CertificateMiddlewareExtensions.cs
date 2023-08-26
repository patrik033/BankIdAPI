using BankAPI.Middleware;

namespace BankAPI.Extensions
{
    public static class CertificateMiddlewareExtensions
    {
        public static IApplicationBuilder UseCertificateHandler(this IApplicationBuilder app,string certFilePath)
        {
            return app.UseMiddleware<CertificateMiddleware>(certFilePath);
        }
    }
}
