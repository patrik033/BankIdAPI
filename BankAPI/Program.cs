
using BankAPI.Extensions;
using Contracts.Implementations;
using Contracts.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
//extensions
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureCertificateHandler();
builder.Services.ConfigureBankIdAuthenticationService(builder.Configuration);
builder.Services.ConfigureDeviceMapper();
builder.Services.ConfigureCorse();

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2V1hhQlJAfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5ad0ZiXHtedXFTR2Ve");


builder.Services.AddSingleton<IBankIdAuthenticationService>(provider =>
{
    var config = builder.Configuration;
    return new BankIdAuthenticationService(config); // Replace with your actual secret key
});
builder.Services.AddSingleton<IDeviceMapper, DeviceMapper>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
