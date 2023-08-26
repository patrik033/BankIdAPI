using Contracts.Implementations;
using Contracts.Interfaces;
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
                var path = Path.Combine(basePath,"Certificates", "FPTestcert4.p12");
                return new CertificateHandler(path);
            });
        }

        public static void ConfigureBankIdAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IBankIdAuthenticationService>(provider =>
            {
                return new BankIdAuthenticationService(configuration);
            });
        }

        public static void ConfigureDeviceMapper(this IServiceCollection services)
        {
            services.AddSingleton<IDeviceMapper,DeviceMapper>();
        }
    }
}
