using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Contracts.Services;
using Entities.Models;
using Entities.Models.Response;
using Contracts.Interfaces.ControllerHelpers;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BankAuthController : ControllerBase
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IBaseAddress _baseAddress;

        public BankAuthController
            (IHttpClientService httpClientService,
            IErrorHandlingService errorHandlingService,
            IBaseAddress baseAddress)
        {
            _httpClientService = httpClientService;
            _errorHandlingService = errorHandlingService;
            _baseAddress = baseAddress;
        }

        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] EndUserIp endUserIp)
        {
            try
            {
                string baseAddress = _baseAddress.GetBaseAddress();

                using var httpClients = _httpClientService.GetConfiguredClient(baseAddress);
                using var request = new HttpRequestMessage(HttpMethod.Post, "auth");
                {
                    ReturnWithoutEncoding(endUserIp, request);
                    var response = await httpClients.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent);
                        return Ok(apiResponse);
                    }
                    return BadRequest(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return _errorHandlingService.HandleException(ex);
            }
        }

        private static void ReturnWithoutEncoding(EndUserIp endUserIp, HttpRequestMessage request)
        {
            var jsonData = JsonSerializer.Serialize(endUserIp);
            var jsonContent = new JsonContentWithoutEncoding(jsonData);
            request.Content = jsonContent;
        }
    }
}