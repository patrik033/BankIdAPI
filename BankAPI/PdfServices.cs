using Entities.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;


namespace BankAPI
{
    public class PdfServices
    {
        public byte[] SignPdf(IFormFile pdfFile, X509Certificate2 certificate,string imagePath,userData? userData)
        {


            using (PdfLoadedDocument loadedDocument = new PdfLoadedDocument(pdfFile.OpenReadStream()))
            {
                //Add a new page to be signed
                PdfPageBase  newPage = loadedDocument.Pages.Add(); 
                PdfGraphics graphics = newPage.Graphics;


                PdfCertificate pdfCertificate = new PdfCertificate(certificate);
                PdfSignature signature = new PdfSignature(loadedDocument, newPage, pdfCertificate, userData.name);
                signature.Bounds = new RectangleF(new PointF(220, 10), new SizeF(300,150));



                byte[] imageBytes = File.ReadAllBytes(imagePath);
                using (MemoryStream imageStream = new MemoryStream(imageBytes))
                {
                    PdfBitmap signatureImage = new PdfBitmap(imageStream);
                    graphics.DrawImage(signatureImage, new PointF(10,10));
                }

                DateTime currentTime = DateTime.Now;
                // Set signature information (contact info, location, and reason).


                signature.SignedName = userData.name;
                signature.ContactInfo = "johndoe@example.com";
                signature.LocationInfo = "Honolulu, Hawaii";
                signature.Reason = $"Signed file {pdfFile.Name}";
                signature.Certificated = true;
                
                

                string signatureDetails = $"Signed date: {currentTime}\n" +
                    $"Signer name: {signature.SignedName}\n" +
                    $"Contact info: {signature.ContactInfo}\n" +
                    $"Location info {signature.LocationInfo}\n" +
                    $"Signing reason: {signature.Reason}";
                
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                graphics.DrawString(signatureDetails, font,PdfBrushes.Black,new PointF(230,30));

                // Draw the signature image.
             

                // Create a stream to save the signed PDF.
                MemoryStream stream = new MemoryStream();
                loadedDocument.Save(stream);
                stream.Position = 0;
                loadedDocument.Close(true);

                // Convert the signed PDF to a byte array and return it.
                return stream.ToArray();
            }
        }
    }
}
