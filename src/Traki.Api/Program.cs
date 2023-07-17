using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Serilog;
using Traki.Api;
using Traki.Domain.Constants;
using Traki.Domain.Handlers;
using Traki.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
services.AddValidatorsFromAssemblyContaining(typeof(Program));

// Custom services registration
Startup.ConfigureServices(services, configuration);

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    /*
    var serviceProvider = app.Services.CreateScope().ServiceProvider;
    serviceProvider.AddInitialData(true);

    var generator = serviceProvider.GetRequiredService<IReportGenerator>();
    const string protocolTemplateName = "Protocol.cshtml";
    try
    {
        await generator.GenerateHtmlReport(new { }, protocolTemplateName);
    } catch(Exception ex)
    {
        Console.WriteLine(ex);
    }
    */
    app.UseCors(Policy.DevelopmentCors);
}


//app.UseHttpsRedirection();

app.UseProblemDetails();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }