using System.Security.Cryptography.X509Certificates;
using Entities.Models;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;



namespace BankAPI
{
    public class SignPdfWithDetailsAndImage
    {
        public byte[] SignPdf(IFormFile pdfFile, X509Certificate2 certificate, string imagePath, userData? userData)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (PdfWriter pdfWriter = new PdfWriter(outputStream))
                {
                    using (PdfDocument pdfDocument = new PdfDocument(pdfWriter))
                    {
                        PdfPage page = pdfDocument.AddNewPage();

                        PdfCanvas pdfCanvas = new PdfCanvas(page, false);

                        byte[] imageBytes = File.ReadAllBytes(imagePath);
                        using (MemoryStream imageStream = new MemoryStream(imageBytes))
                        {
                            PdfImageXObject imageXObject = new PdfImageXObject(ImageDataFactory.Create(imageStream.ToArray()));
                            pdfCanvas.AddXObjectAt(imageXObject, 10, 10);
                        }

                        DateTime currentTime = DateTime.Now;

                        string signatureDetails = $"Signed date: {currentTime}\n" +
                            $"Signer name: {userData?.name}\n" +
                            $"Contact info: johndoe@example.com\n" +
                            $"Location info: Honolulu, Hawaii\n" +
                            $"Signing reason: Signed file {pdfFile.Name}";

                        PdfFont defaultFont = PdfFontFactory.CreateFont();
                        pdfCanvas.BeginText().SetFontAndSize(defaultFont, 12).MoveText(230, 30).ShowText(signatureDetails).EndText();

                        pdfDocument.Close();
                    }
                }

                // Get the signed PDF as a byte array
                byte[] signedPdfBytes = outputStream.ToArray();

                // Sign the PDF
                //SignPdfDocument(signedPdfBytes, certificate);

                return signedPdfBytes;
            }
        }

        //public byte[] SignPdfDocument(byte[] pdfBytes, X509Certificate2 certificate)
        //{
        //    using (MemoryStream signedPdfStream = new MemoryStream())
        //    {
        //        PdfReader pdfReader = new PdfReader(new MemoryStream(pdfBytes));
        //        PdfSigner pdfSigner = new PdfSigner(pdfReader, signedPdfStream, new StampingProperties());

        //        // Set the signature appearance
        //        pdfSigner.SetFieldName("sig");
        //        pdfSigner.GetSignatureAppearance().SetPageNumber(1);

        //        // Create the signature
        //        pdfSigner.SignDetached(new X509Certificate2Signature(certificate, DigestAlgorithms.SHA256), new[] { certificate }, null, null, null, 0, PdfSigner.CryptoStandard.CMS);

        //        return signedPdfStream.ToArray();
        //    }
        //}
    }
}
