using Contracts.Services;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Contracts.Implementations
{
    public class JsonContentWithoutEncoding : HttpContent,IJsonContentWithoutEncoding
    {
        //private readonly string JsonData;
        public string JsonData { get; }
        public JsonContentWithoutEncoding(string jsonData)
        {
            jsonData = JsonData;
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        //public string JsonData => throw new NotImplementedException();

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            using (var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            {
                await writer.WriteAsync(JsonData);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1; // We don't know the length beforehand
            return false;
        }
    }
}
