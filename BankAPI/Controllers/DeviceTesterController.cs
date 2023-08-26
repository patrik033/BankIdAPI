using BankAPI.Models.Device;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UAParser;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTesterController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetDevice()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo info = uaParser.Parse(userAgent);

            var deviceMapper = new DeviceMapper();
            Devices deviceType = deviceMapper.MapDevice(info);

            return Ok($"Detected device: {deviceType}\nVersion: {info.OS.Major}");
        }
    }
}
