using Contracts.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace Contracts.Implementations
{
    public class CertificateHandler : ICertificateHandler
    {

        private X509Certificate2 _certificate;
        private readonly string _certFilePath;
        private bool _isCertificateLoaded;
        public CertificateHandler(string certFilePath)
        {
            _certFilePath = certFilePath;
            _isCertificateLoaded = false;
        }


        public void AddCertificateToStore(X509Certificate2 certificate, StoreLocation location, StoreName storeName)
        {
            var store = new X509Store(storeName, location);

            // Öppna butiken för ändringar
            store.Open(OpenFlags.ReadWrite);

            try
            {
                // Kontrollera om certifikatet redan finns i butiken
                var existingCertificate = store.Certificates.Find(X509FindType.FindByThumbprint, certificate.Thumbprint, validOnly: false);
                if (existingCertificate.Count == 0)
                {
                    // Lägg bara till certifikatet om det inte redan finns i butiken
                    store.Add(certificate);
                }
                else
                {
                    Console.WriteLine("Certificate already exists in the store.");
                }
            }
            catch (Exception ex)
            {
                // Hantera fel här om något går fel
                Console.WriteLine($"Error adding certificate to store: {ex.Message}");
            }
            finally
            {
                // Stäng butiken efter ändringar
                store.Close();
            }
        }

        public bool AddCertificateToStores()
        {
            if (!_isCertificateLoaded)
            {

                try
                {
                    // Läs in certifikatet från filen
                    var certificate = new X509Certificate2(_certFilePath,"qwerty123");

                    // Lägg till certifikatet i användarens certifikatbutik (CurrentUser)
                    AddCertificateToStore(certificate, StoreLocation.CurrentUser, StoreName.Root);

                    // Lägg till certifikatet i roten certifikatbutik (LocalMachine)
                    //AddCertificateToStore(certificate, StoreLocation.LocalMachine, StoreName.Root);

                    return true;
                }
                catch (Exception ex)
                {
                    // Hantera fel här om något går fel
                    Console.WriteLine($"Fel vid hantering av certifikat: {ex.Message}");
                    return false;
                }
            }
            return false;
        }



        private void LoadCertificateIfNeeded()
        {
            if (!_isCertificateLoaded)
            {
                try
                {
                    string certPassword = "qwerty123"; // Replace with the actual password for your certificate

                    X509Certificate2Collection certificates = new X509Certificate2Collection();
                    certificates.Import(_certFilePath, certPassword, X509KeyStorageFlags.PersistKeySet);

                    if (certificates.Count > 0)
                    {
                        // Return the first certificate in the collection (if any)
                        _certificate = certificates[0];
                        _isCertificateLoaded = true;
                    }
                    else
                    {
                        Console.WriteLine("No valid certificate found in the file.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading the certificate: {ex.Message}");
                }
            }
        }


        public X509Certificate2 GetCertificate2()
        {
            //AddCertificateToStores();
            LoadCertificateIfNeeded();
            return _certificate;
        }
    }
}
