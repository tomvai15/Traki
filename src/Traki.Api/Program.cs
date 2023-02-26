using Serilog;
using Traki.Api;
using Traki.Api.Bootstrapping;
using Traki.Api.Constants;
using Traki.Api.Data;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

// Custom services registration
Startup.ConfigureServices(services, configuration);

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.AddInitialData();
}

app.UseCors(Policy.DevelopmentCors);

// TODO: temporary fix for emulator. Need to figure out how to make https work on emulator
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }