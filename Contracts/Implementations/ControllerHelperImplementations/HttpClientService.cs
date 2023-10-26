using Contracts.Interfaces.ControllerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Implementations.ControllerHelperImplementations
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientHandlerService _httpClientHandlerService;

        public HttpClientService(IHttpClientHandlerService httpClientHandlerService)
        {
            _httpClientHandlerService = httpClientHandlerService ?? throw new ArgumentNullException(nameof(httpClientHandlerService));
        }

        public HttpClient GetConfiguredClient(string baseAddress)
        {

            var handler = _httpClientHandlerService.GetConfiguredHandler();

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseAddress),
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
    }
}
