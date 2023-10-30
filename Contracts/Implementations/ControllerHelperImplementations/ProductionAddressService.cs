using Contracts.Interfaces.ControllerHelpers;
using Contracts.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Implementations.ControllerHelperImplementations
{
    public class ProductionAddressService : IBaseAddress
    {
        private readonly string _productionAddress;

        public ProductionAddressService(string productionAddress )
        {
            _productionAddress = productionAddress ?? throw new ArgumentNullException(nameof(productionAddress));
        }

        public string GetBaseAddress()
        {
            return _productionAddress;
        }
    }
}
