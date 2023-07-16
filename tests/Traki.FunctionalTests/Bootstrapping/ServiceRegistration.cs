using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using Traki.FunctionalTests.Extensions;

namespace Traki.FunctionalTests.Bootstrapping
{
    public static class ServiceRegistration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IWebDriver>((_) => BuildDriver(configuration.LaunchBrowser()));

            return services;
        }
    }
}
