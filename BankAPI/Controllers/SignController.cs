using Contracts.Interfaces;
using Contracts.Services;
using Entities.Models;
using Entities.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;


namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICertificateHandler _certificateHandler;
        public SignController(IHttpClientFactory httpClientFactory, ICertificateHandler certificateHandler)
        {
            _httpClientFactory = httpClientFactory;
            _certificateHandler = certificateHandler;
        }

        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] SignRequest signRequest)
        {
            try
            {
                var certificate = _certificateHandler.GetCertificate2();
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(certificate);

                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var httpClient = new HttpClient(handler);
                httpClient.BaseAddress = new Uri("https://appapi2.test.bankid.com/rp/v6.0/");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var request = new HttpRequestMessage(HttpMethod.Post, "sign"))
                {

                    var jsonData = JsonSerializer.Serialize(signRequest);
                    var jsonContent = new JsonContentWithoutEncoding(jsonData);
                    request.Content = jsonContent;

                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent);
                        return Ok(apiResponse);
                    }
                    return BadRequest(await response.Content.ReadAsStringAsync());
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle general API request errors
                return StatusCode(500, $"API request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions that may occur during the API call.
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("upload")]
        public IActionResult UploadFile(IFormFile file, [FromForm] string user)
        {
            userData userData = JsonSerializer.Deserialize<userData>(user);
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            var cert = Path.Combine(basePath, "Certificates", "client-identity.p12");
            var img = Path.Combine(basePath, "Certificates", "toys-small.jpg");
            X509Certificate2 certificate = new X509Certificate2(cert, "password");

            var signPdf = new PdfServices();

            byte[] signedPdfBytes = signPdf.SignPdf(file, certificate, img,userData);
            return File(signedPdfBytes, "application/pdf", "SignedOutput.pdf");
        }

        [HttpPost]
        [Route("signpdf")]
        public IActionResult SignPdf(IFormFile file, [FromForm] string user)
        {
            userData userData = JsonSerializer.Deserialize<userData>(user);
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            var cert = Path.Combine(basePath, "Certificates", "client-identity.p12");
            var img = Path.Combine(basePath, "Certificates", "toys-small.jpg");
            X509Certificate2 certificate = new X509Certificate2(cert, "password");
       
                var pdfService = new PdfServices();
   
                byte[] signedPdfBytes = pdfService.SignPdf(file, certificate,img, userData);
                return File(signedPdfBytes, "application/pdf", "SignedOutput.pdf");
        }

        [HttpGet]
        public IActionResult GetFile()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(basePath, "Certificates", "Sorting.pdf");
            var fileContens = System.IO.File.ReadAllBytes(path);
            return File(fileContens, "application/pdf", "Sorting.pdf");
        }
    }
}
