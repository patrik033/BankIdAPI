using Entities.Models;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using Contracts.Interfaces;
using Entities.Models.Response;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICertificateHandler _certificateHandler;
        private readonly IConfiguration _configuration;
        private readonly IBankIdAuthenticationService _bankIdAuthenticationService;

        public CollectController
            (IHttpClientFactory httpClientFactory,
            ICertificateHandler certificateHandler,
            IConfiguration configuration,
            IBankIdAuthenticationService bankIdAuthenticationService
            )
        {
            _httpClientFactory = httpClientFactory;
            _certificateHandler = certificateHandler;
            _configuration = configuration;
            _bankIdAuthenticationService = bankIdAuthenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CollectPost([FromBody] OrderRef orderRef)
        {
            const string testBaseAddress = "https://appapi2.test.bankid.com/rp/v6.0/";

            try
            {
                // Load the client certificate from a file or any secure storage.
                var certificate = _certificateHandler.GetCertificate2();

                // Create an HttpClientHandler and assign the client certificate to it.
                var clientHandler = new HttpClientHandler();
                clientHandler.ClientCertificates.Add(certificate);
                // Disable SSL certificate validation (Not recommended for production use!)
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                // Ensure TLS 1.2 is used for the API call.
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                // Instantiate the HttpClient with the custom HttpClientHandler.
                var httpClient = new HttpClient(clientHandler);
                httpClient.BaseAddress = new Uri(testBaseAddress);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // API endpoint URL for the POST request.
                using (var request = new HttpRequestMessage(HttpMethod.Post, "collect"))
                {
                    ReturnWithoutEncoding(orderRef, request);
                    var response = await httpClient.SendAsync(request);
                    return await NewMethod(response);
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

        private async Task<IActionResult> NewMethod(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<CollectResponse>(responseContent); // Deserialize the response JSON

                if (responseObject.status == "complete" && responseObject.completionData != null)
                {
                    var authService = _bankIdAuthenticationService;
                    var jwtToken = authService.GenerateJwtToken(responseObject.completionData);
                    // Return the JWT token as a response
                    return Ok(new { status = responseObject.status, token = jwtToken, response = responseObject });
                }
                else
                {
                    return Ok(responseContent);
                }
            }
            else
            {
                //fropntend will handle here
                var content = await response.Content.ReadAsStringAsync();
                return BadRequest(content);
            }
        }

        private static void ReturnWithoutEncoding(OrderRef orderRef, HttpRequestMessage request)
        {
            var jsonData = JsonSerializer.Serialize(orderRef);
            var jsonContent = new JsonContentWithoutEncoding(jsonData);
            request.Content = jsonContent;
        }
    }
}
