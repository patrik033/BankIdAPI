using Entities.Models;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Contracts.Interfaces.ControllerHelpers;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CancelController : ControllerBase
    {

        private readonly IHttpClientService _httpClientService;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IBaseAddress _baseAddress;

        public CancelController(
            IHttpClientService httpClientService,
            IErrorHandlingService errorHandlingService,
            IBaseAddress baseAddress)
        {
            _httpClientService = httpClientService;
            _errorHandlingService = errorHandlingService;
            _baseAddress = baseAddress;
        }

        [HttpPost]
        public async Task<IActionResult> PostCollect([FromBody] OrderRef orderRef)
        {
            try
            {
                string baseAddress = _baseAddress.GetBaseAddress();
                using var httpClients = _httpClientService.GetConfiguredClient(baseAddress);
                using var request = new HttpRequestMessage(HttpMethod.Post, "cancel");

                UtilityHelpers.ReturnWithoutEncoding(orderRef, request);

                var response = await httpClients.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return Ok(responseContent);
                }
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                // Handle other exceptions that may occur during the API call.
                return _errorHandlingService.HandleException(ex);
            }
        }
    }
}
