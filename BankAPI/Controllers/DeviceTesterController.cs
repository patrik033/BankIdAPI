using Contracts.Implementations;
using Contracts.Interfaces;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using UAParser;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTesterController : ControllerBase
    {
        private readonly IDeviceMapper _deviceMapper;

        public DeviceTesterController(IDeviceMapper deviceMapper)
        {
            _deviceMapper = deviceMapper;
        }

        [HttpGet]
        public IActionResult GetDevice()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo info = uaParser.Parse(userAgent);

            var deviceMapper = _deviceMapper;
            Devices deviceType = deviceMapper.MapDevice(info);

            return Ok($"Detected device: {deviceType}\nVersion: {info.OS.Major}");
        }
    }
}
