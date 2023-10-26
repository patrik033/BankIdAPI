
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Contracts.Services;
using Entities.Models;
using Contracts.Interfaces;
using Entities.Models.Response;
using Contracts.Interfaces.ControllerHelpers;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class BankAuthController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICertificateProvider _certificateProvider;
        private readonly IHttpClientService _httpClientService;
        private readonly IErrorHandlingService _errorHandlingService;


        public BankAuthController
            (IHttpClientFactory httpClientFactory,
            ICertificateProvider certificateProvider,
            IHttpClientService httpClientService,
            IErrorHandlingService errorHandlingService)
        {
            _httpClientFactory = httpClientFactory;
            _certificateProvider = certificateProvider;
            _httpClientService = httpClientService;
            _errorHandlingService = errorHandlingService;
        }

        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] EndUserIp endUserIp)
        {
            try
            {
                const string baseAddress = "https://appapi2.test.bankid.com/rp/v6.0/";
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