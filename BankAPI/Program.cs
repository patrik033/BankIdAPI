
using BankAPI.Extensions;
using Contracts.Implementations;
using Contracts.Interfaces;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
//extensions
builder.Services.ConfigureAuthentication(builder.Configuration);

//initialize the azureCertificate
//since it can take some time to retrieve the certificate it best done when we start the app
//await builder.Services.ConfigureAzureCertificateHandler("https://some.vault.azure.net/", "Bank");
builder.Services.ConfigureCertificateHandler();
builder.Services.ConfigureBankIdAuthenticationService(builder.Configuration);

builder.Services.ConfigureCorse();

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2V1hhQlJAfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5ad0ZiXHtedXFTR2Ve");

builder.Services.AddSingleton<IBankIdAuthenticationService>(provider =>
{
    var config = builder.Configuration;
    return new BankIdAuthenticationService(config); // Replace with your actual secret key
});

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
