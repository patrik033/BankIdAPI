using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IAzureCertificate
    {
        public X509Certificate2 GetCertificate();
    }
}
