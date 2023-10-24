using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Implementations.ExtensionImplementations
{
    public class LocalCertificateProvider : ICertificateProvider
    {

        private readonly ICertificateHandler _certificateHandler;

        public LocalCertificateProvider(ICertificateHandler certificateHandler)
        {
            _certificateHandler = certificateHandler;
        }

        public X509Certificate2 GetCertificate()
        {
            return _certificateHandler.GetCertificate2();
        }
    }
}
