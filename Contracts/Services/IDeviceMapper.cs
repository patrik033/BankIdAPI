using Entities.Models;
using UAParser;

namespace Contracts.Services
{
    public interface IDeviceMapper
    {
        Devices MapDevice(ClientInfo clientInfo);
    }
}