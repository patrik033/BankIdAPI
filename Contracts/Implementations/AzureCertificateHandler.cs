using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Implementations
{
    public class AzureCertificateHandler : IAzureCertificate, IAzureCertificateInitializer
    {
        private bool _isInitialized = false;

        private X509Certificate2 _certificate;
        private SecretClient _secretClient;
        private KeyVaultSecret _keyVaultSecret;

        public async Task Initialize(string keyVaultUrl, string certificateName)
        {
            if (_isInitialized)
            {
                return;
            }

            _secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            _keyVaultSecret = await _secretClient.GetSecretAsync(certificateName);
            _isInitialized = true;
            _certificate = new X509Certificate2(Convert.FromBase64String(_keyVaultSecret.Value));
        }

        public X509Certificate2 GetCertificate()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Initialize must be called before accessing the certificate.");
            }

            return _certificate;
        }
    }
}
