using System.Security.Cryptography.X509Certificates;

namespace BankAPI.Services
{
    public interface ICertificateHandler
    {
        bool AddCertificateToStores();
        void AddCertificateToStore(X509Certificate2 certificate, StoreLocation location,StoreName storeName);
        public X509Certificate2 GetCertificate2();
    }
}
