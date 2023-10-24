using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Implementations.ExtensionImplementations
{
    public class AzureCertificateProvider : ICertificateProvider
    {

        private readonly IAzureCertificate _azureCertificate;

        public AzureCertificateProvider(IAzureCertificate azureCertificate)
        {
            _azureCertificate = azureCertificate;
        }
        public X509Certificate2 GetCertificate()
        {
            return _azureCertificate.GetCertificate();
        }
    }
}
