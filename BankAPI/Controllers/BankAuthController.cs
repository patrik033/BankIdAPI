
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Contracts.Services;

using Entities.Models;
using BankAPI.Models;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class BankAuthController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICertificateHandler _certificateHandler;

        public BankAuthController(IHttpClientFactory httpClientFactory,ICertificateHandler certificateHandler)
        {
            _httpClientFactory = httpClientFactory;
            _certificateHandler = certificateHandler;
        }

        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] EndUserIp endUserIp)
        {
            try
            {
                // Load the client certificate from a file or any secure storage.
                var certificate =  _certificateHandler.GetCertificate2();

                // Create an HttpClientHandler and assign the client certificate to it.
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(certificate);
                // Disable SSL certificate validation (Not recommended for production use!)
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                // Ensure TLS 1.2 is used for the API call.
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Instantiate the HttpClient with the custom HttpClientHandler.
                var httpClient = new HttpClient(handler);
                httpClient.BaseAddress = new Uri("https://appapi2.test.bankid.com/rp/v6.0/");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // API endpoint URL for the POST request.
                using (var request = new HttpRequestMessage(HttpMethod.Post, "auth"))
                {
                    var jsonData = JsonSerializer.Serialize(endUserIp);
                    var jsonContent = new JsonContentWithoutEncoding(jsonData);
                    request.Content = jsonContent;
                    var response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<ApiAuthSuccessResponse>(responseContent);
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

        [HttpGet]
        public  IActionResult GetCertificate()
        {
            var loadedCertificate = _certificateHandler.GetCertificate2();
            if (loadedCertificate == null)
            {
                return BadRequest("No files were found");
            }

            // Return the certificate file with the content type "application/x-pkcs12"
            return File(loadedCertificate.RawData, "application/x-pkcs12", "certificate.pfx");
        }

      
    }
}