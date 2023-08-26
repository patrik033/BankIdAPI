using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Contracts.Services
{
    public class JsonContentWithoutEncoding : HttpContent
    {
        private readonly string _json;

        public JsonContentWithoutEncoding(string json)
        {
            _json = json;
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            using (var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            {
                await writer.WriteAsync(_json);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1; // We don't know the length beforehand
            return false;
        }
    }
}
