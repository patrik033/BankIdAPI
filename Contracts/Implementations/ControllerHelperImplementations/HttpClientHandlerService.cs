using Contracts.Interfaces;
using Contracts.Interfaces.ControllerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Implementations.ControllerHelperImplementations
{
    public class HttpClientHandlerService : IHttpClientHandlerService
    {
        private readonly ICertificateProvider _certificateProvider;

        public HttpClientHandlerService(ICertificateProvider certificateProvider)
        {
            _certificateProvider = certificateProvider ?? throw new ArgumentNullException(nameof(certificateProvider));
        }

        public HttpClientHandler GetConfiguredHandler()
        {
            var certificate = _certificateProvider.GetCertificate();

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate);
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return handler;
        }
    }
}
