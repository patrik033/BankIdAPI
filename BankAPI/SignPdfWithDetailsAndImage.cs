using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf;
using System.Security.Cryptography.X509Certificates;
using Syncfusion.Drawing;

namespace BankAPI
{
    public class SignPdfWithDetailsAndImage
    {
        public byte[] SignExistingPdf(Stream pdfStream, X509Certificate2 certificate, Stream ?signatureImageStream)
        {
            // Load the input PDF
            using (PdfLoadedDocument document = new PdfLoadedDocument(pdfStream))
            {
                PdfLoadedPage page = document.Pages[0] as PdfLoadedPage;
                PdfGraphics graphics = page.Graphics;

                // Create the digital signature
                PdfCertificate pdfCert = new PdfCertificate(certificate);
                PdfSignature signature = new PdfSignature(document, page, pdfCert, "Signature");

                PdfSignatureSettings settings = signature.Settings;
                settings.DigestAlgorithm = DigestAlgorithm.SHA256;

                // Set and draw the visual representation of the signature
                PdfBitmap signatureImage = new PdfBitmap(signatureImageStream);
                signature.Bounds = new RectangleF(new PointF(0, 0), signatureImage.PhysicalDimension);
                graphics.DrawImage(signatureImage, 0, 0);

                signature.ContactInfo = "johndoe@owned.us";
                signature.LocationInfo = "Honolulu, Hawaii";
                signature.Reason = "I am author of this document.";

                using (MemoryStream signedPdfStream = new MemoryStream())
                {
                    document.Save(signedPdfStream);
                    return signedPdfStream.ToArray();
                }
            }
        }
    }
}
