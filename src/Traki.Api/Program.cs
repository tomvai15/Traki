using Serilog;
using Traki.Api.Bootstrapping;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration config = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

services.AddMappingProfiles()
        .AddTrakiDbContext(config)
        .AddAuthorisationServices(config);

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
