using Entities.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IBankIdAuthenticationService
    {
        public string GenerateJwtToken(CompletionData completionData);
    }
}
