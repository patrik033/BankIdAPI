using System.Text.Json;

namespace Contracts.Services
{
    public static class UtilityHelpers
    {
        public static void ReturnWithoutEncoding<T>(T data, HttpRequestMessage request)
        {
            var jsonData = JsonSerializer.Serialize(data);
            var jsonContent = new JsonContentWithoutEncoding(jsonData);
            request.Content = jsonContent;
        }
    }
}
