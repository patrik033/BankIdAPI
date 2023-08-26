using BankAPI.Models;
using BankAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using BankAPI.Models.User;
using BankAPI.Models.User.Response;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICertificateHandler _certificateHandler;
        private readonly IConfiguration _configuration;

        public CollectController(IHttpClientFactory httpClientFactory, ICertificateHandler certificateHandler, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _certificateHandler = certificateHandler;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CollectPost([FromBody] OrderRef orderRef)
        {
            try
            {
                // Load the client certificate from a file or any secure storage.
                var certificate = _certificateHandler.GetCertificate2();

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
                using (var request = new HttpRequestMessage(HttpMethod.Post, "collect"))
                {
                    var jsonData = JsonSerializer.Serialize(orderRef);
                    var jsonContent = new JsonContentWithoutEncoding(jsonData);
                    request.Content = jsonContent;

                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonSerializer.Deserialize<ApiResponseCollect>(responseContent); // Deserialize the response JSON

                        if (responseObject.status == "complete" && responseObject.completionData != null)
                        {
                            var authService = new BankIdAuthenticationService(_configuration); // Replace with your actual secret key
                            var jwtToken = authService.GenerateJwtToken(responseObject.completionData);

                            // Return the JWT token as a response
                            return Ok(new { status = responseObject.status,token = jwtToken });
                        }
                        else
                        {
                            // Handle other cases if needed
                            return Ok(responseContent);
                        }
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return BadRequest(content);
                    }
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
    }
}
