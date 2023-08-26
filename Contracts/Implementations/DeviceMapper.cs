using Contracts.Services;
using Entities.Models;
using UAParser;

namespace Contracts.Implementations
{
    public class DeviceMapper : IDeviceMapper
    {
        private Dictionary<string, Devices> _osToDeviceMap;

        public DeviceMapper()
        {
            _osToDeviceMap = new Dictionary<string, Devices>
          {
            {"Android",Devices.Android },
            {"BlackBerry OS",Devices.BlackBerryOS },
            {"CentOs",Devices.CentOS },
            {"Chrome OS",Devices.ChromeOS },
            {"Fedora",Devices.Fedora },
            {"GoogleTV",Devices.GoogleTV },
            {"iOS",Devices.iOS },
            {"Linux Mint",Devices.LinuxMint },
            {"Mac OS X",Devices.MacOSX },
            {"Mandriva",Devices.Mandriva },
            {"PCLinuxOS",Devices.PCLinuxOS },
            {"Puppy",Devices.Puppy },
            {"Red Hat",Devices.RedHat },
            {"Slackware",Devices.Slackware },
            {"Symbian OS",Devices.SymbianOS },
            {"Symbian^3",Devices.Symbian3 },
            {"Windows Phone",Devices.WindowsPhone },
            {"Linux",Devices.Linux },
            {"Mac OS",Devices.MacOS },
            {"Windows",Devices.Windows },
          };


        }

        public Devices MapDevice(ClientInfo clientInfo)
        {
            string osFamily = clientInfo.OS.Family;

            if (_osToDeviceMap.TryGetValue(osFamily, out Devices devicesType))
            {
                return devicesType;
            }
            return Devices.Unknown;
        }
    }
}
