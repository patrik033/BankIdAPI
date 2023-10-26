using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces.ControllerHelpers
{
    public interface IHttpClientService
    {
        HttpClient GetConfiguredClient(string baseAddress);
    }
}
