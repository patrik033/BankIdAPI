using Contracts.Implementations;
using Contracts.Implementations.ControllerHelperImplementations;
using Contracts.Implementations.ExtensionImplementations;
using Contracts.Interfaces;
using Contracts.Interfaces.ControllerHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BankAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCorse(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidIssuer = configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });
        }

        public static void ConfigureCertificateHandler(this IServiceCollection services)
        {
            services.AddSingleton<ICertificateHandler>(provider =>
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                var path = Path.Combine(basePath, "Certificates", "FPTestcert4.p12");
                return new CertificateHandler(path);
            });

            services.AddSingleton<ICertificateProvider, LocalCertificateProvider>();
        }

        public static async Task ConfigureAzureCertificateHandler(this IServiceCollection services, string keyVaultUrl, string certificateName)
        {
            var certificateHandler = new AzureCertificateHandler();
            await certificateHandler.Initialize(keyVaultUrl, certificateName);

            services.AddSingleton<IAzureCertificate>(certificateHandler);
            services.AddSingleton<ICertificateProvider, AzureCertificateProvider>();
        }



        public static void ConfigureBankIdAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IBankIdAuthenticationService>(provider =>
            {
                return new BankIdAuthenticationService(configuration);
            });
        }

        public static void UseTestAddress(this IServiceCollection services, IConfiguration configuration)
        {
            var testAddress = configuration.GetValue<string>("BaseAddressConfiguration:TestAddress");
            services.AddSingleton<IBaseAddress>(new TestAddressService(testAddress));
        }

        /// <summary>
        /// Uses the BankIds production address.
        /// To be able to use this make sure you are using a production certificate or the service will throw an 500 error with no message
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void UseProductionAddress(this IServiceCollection services, IConfiguration baseAddressConfiguration)
        {
            var productionAddress = baseAddressConfiguration.GetValue<string>("BaseAddressConfiguration:ProductionAddress");
            services.AddSingleton<IBaseAddress>(new ProductionAddressService(productionAddress));
        }

    }
}
