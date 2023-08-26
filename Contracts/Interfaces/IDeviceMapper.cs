using Entities.Models;
using UAParser;

namespace Contracts.Interfaces
{
    public interface IDeviceMapper
    {
        Devices MapDevice(ClientInfo clientInfo);
    }
}