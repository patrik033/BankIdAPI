using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Contracts.Services
{
    public interface IJsonContentWithoutEncoding
    {
         string JsonData { get; }
    }
}
