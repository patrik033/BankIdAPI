using Contracts.Implementations;
using Entities.Models.Response;
using Microsoft.Extensions.Configuration;

namespace BankIdTests
{
    public class BankIdAuthenticationServiceTests
    {
        [Fact]
        public void GenerateJwtToken_ReturnsJwtToken()
        {


            // Arrange
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        {
            {"Jwt:Key", "yourSecretKey"},
            {"Jwt:Issuer", "yourIssuer"},
            {"Jwt:Audience", "yourAudience"}
        }).Build();

            var bankIdAuthenticationService = new BankIdAuthenticationService(configuration);
            var completionData = new CompletionData
            {
                bankIdIssueDate = DateTime.UtcNow.ToString(),
                ocspResponse = "yourOcspResponse",
                // user = new User { givenName = "John", name = "Doe", surname = "Smith" }
            };

            // Act
            var jwtToken = bankIdAuthenticationService.GenerateJwtToken(completionData);

            // Assert
            Assert.NotNull(jwtToken);
            // Assert other expectations for the generated token
        }

        // Add more test methods for different scenarios
    }
}
