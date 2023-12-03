using Entities.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace BankAPI
{
    public class PdfServices
    {
        public byte[] SignPdf(IFormFile pdfFile, X509Certificate2 certificate, string imagePath, userData? userData)
        {
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(pdfFile.OpenReadStream());
            PdfPageBase page = loadedDocument.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            PdfCertificate certificatePdf = new PdfCertificate(certificate);
            PdfSignature signature = new PdfSignature(loadedDocument, page, certificatePdf, "Signature");


            signature.Bounds = new RectangleF(new PointF(220, 10), new SizeF(300, 200));

            FileStream imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            PdfBitmap signatureImage = new PdfBitmap(imageStream);

            //signature.Certificate = certificatePdf;
            signature.ContactInfo = "johndoe@owned.us";
            signature.LocationInfo = "Honolulu, Hawaii";
            signature.Reason = "I am the author of this document";
            signature.SignedName = userData.name;
            

            PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 15);

            graphics.DrawRectangle(PdfPens.Black, PdfBrushes.White, new RectangleF(50, 0, 200, 200));
            graphics.DrawImage(signatureImage, new RectangleF(0, 0, 100, 100));
            graphics.DrawString($"Digitally Signed by {userData.name}", font, PdfBrushes.Black, 120, 17);
            graphics.DrawString("Reason: Testing signature", font, PdfBrushes.Black, 120, 39);
            graphics.DrawString("Location: Sweden", font, PdfBrushes.Black, 120, 60);


            
            //    // Create a stream to save the signed PDF.
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            stream.Position = 0;
            loadedDocument.Close(true);

            return stream.ToArray();
        }
    }
}
