using Contracts.Interfaces.ControllerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Implementations.ControllerHelperImplementations
{
    public class TestAddressService : IBaseAddress
    {
        private readonly string _testAddress;

        public TestAddressService(string testAddress)
        {
            _testAddress = testAddress ?? throw new ArgumentNullException(nameof(testAddress));
        }

        public string GetBaseAddress()
        {
            return _testAddress;
        }
    }
}
