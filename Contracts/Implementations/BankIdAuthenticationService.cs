using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities.Models.User.Response;
using Microsoft.Extensions.Configuration;
using Contracts.Services;

namespace Contracts.Implementations
{
    public class BankIdAuthenticationService : IBankIdAuthenticationService
    {

        private readonly IConfiguration _configuration;

        public BankIdAuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(CompletionData completionData)
        {
            var claims = new[]
        {
            new Claim("bankIdIssueDate", completionData.bankIdIssueDate),
            new Claim("ocspResponse", completionData.ocspResponse),
            new Claim("userGivenName", completionData.user.givenName),
            new Claim("userName", completionData.user.name),
            new Claim("userSurname", completionData.user.surname)
        };

            var expiresAt = DateTime.UtcNow.AddMinutes(10);
            var accessToken = CreateToken(claims, expiresAt);
            return accessToken;
        }

        private string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims:claims,
                    notBefore: DateTime.UtcNow,
                    expires: expiresAt,
                    signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
