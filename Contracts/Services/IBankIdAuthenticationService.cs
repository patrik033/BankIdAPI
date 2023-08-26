using Entities.Models.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Services
{
    public interface IBankIdAuthenticationService
    {
        public string GenerateJwtToken(CompletionData completionData);
    }
}
