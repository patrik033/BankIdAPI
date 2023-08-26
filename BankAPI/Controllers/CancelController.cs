using BankAPI.Models;
using BankAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CancelController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICertificateHandler _certificateHandler;

        public CancelController(IHttpClientFactory httpClientFactory, ICertificateHandler certificateHandler)
        {
            _httpClientFactory = httpClientFactory;
            _certificateHandler = certificateHandler;
        }

        [HttpPost]
        public async Task<IActionResult> PostCollect([FromBody] OrderRef orderRef)
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

                using (var request = new HttpRequestMessage(HttpMethod.Post, "cancel"))
                {
                    var jsonData = JsonSerializer.Serialize(orderRef);
                    var jsonContent = new JsonContentWithoutEncoding(jsonData);
                    request.Content = jsonContent;

                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return Ok(responseContent);
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
    }
}
