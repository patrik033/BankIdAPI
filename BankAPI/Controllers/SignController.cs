using Contracts.Interfaces.ControllerHelpers;
using Contracts.Services;
using Entities.Models;
using Entities.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignController : ControllerBase
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IBaseAddress _baseAddress;
        public SignController
            (IHttpClientService httpClientService,
            IErrorHandlingService errorHandlingService,
            IBaseAddress baseAddress)
        {
            _httpClientService = httpClientService;
            _errorHandlingService = errorHandlingService;
            _baseAddress = baseAddress;
        }

        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] SignRequest signRequest)
        {

            try
            {
                string baseAddress = _baseAddress.GetBaseAddress();
                using var httpClients = _httpClientService.GetConfiguredClient(baseAddress);
                using var request = new HttpRequestMessage(HttpMethod.Post, "sign");

                ReturnWithoutEncoding(signRequest, request);
                var response = await httpClients.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent);
                    return Ok(apiResponse);
                }
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                // Handle other exceptions that may occur during the API call.
                return _errorHandlingService.HandleException(ex);
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

            byte[] signedPdfBytes = signPdf.SignPdf(file, certificate, img, userData);
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

        private static void ReturnWithoutEncoding(SignRequest signRequest, HttpRequestMessage request)
        {
            var jsonData = JsonSerializer.Serialize(signRequest);
            var jsonContent = new JsonContentWithoutEncoding(jsonData);
            request.Content = jsonContent;
        }
    }
}
