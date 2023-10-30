using Entities.Models;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Contracts.Interfaces;
using Entities.Models.Response;
using Contracts.Interfaces.ControllerHelpers;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectController : ControllerBase
    {
        private readonly IBankIdAuthenticationService _bankIdAuthenticationService;
        private readonly IHttpClientService _httpClientService;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IBaseAddress _baseAddress;

        public CollectController
            (IBankIdAuthenticationService bankIdAuthenticationService,
            IHttpClientService httpClientService,
            IErrorHandlingService errorHandlingService, IBaseAddress baseAddress)
        {
            _bankIdAuthenticationService = bankIdAuthenticationService;
            _httpClientService = httpClientService;
            _errorHandlingService = errorHandlingService;
            _baseAddress = baseAddress;
        }

        [HttpPost]
        public async Task<IActionResult> CollectPost([FromBody] OrderRef orderRef)
        {
            try
            {
                string baseAddress = _baseAddress.GetBaseAddress();

                using (var httpClient = _httpClientService.GetConfiguredClient(baseAddress))
                using (var request = new HttpRequestMessage(HttpMethod.Post, "collect"))
                {
                    ReturnWithoutEncoding(orderRef, request);
                    var response = await httpClient.SendAsync(request);
                    return await ReturnResponseWithToken(response);
                }
            }
            catch (Exception ex)
            {
                return _errorHandlingService.HandleException(ex);
            }
        }

        private async Task<IActionResult> ReturnResponseWithToken(HttpResponseMessage response)
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
                //frontend will handle here
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
