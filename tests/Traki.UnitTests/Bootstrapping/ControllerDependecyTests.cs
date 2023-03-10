using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Traki.Api;
using Traki.Api.Models;

namespace Traki.UnitTests.Bootstrapping
{
    public class ControllerDependecyTests
    {
        [Theory]
        [MemberData(nameof(Controllers))]
        public void CanResolveControllers(Type type)
        {
            // Arrange
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            IConfiguration configuration = configurationBuilder.Build();

            IServiceCollection services = new ServiceCollection();
            Startup.ConfigureServices(services, configuration);

            services.AddSingleton(type);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Act
            using var scope = serviceProvider.CreateScope();
            var controller = scope.ServiceProvider.GetService(type);

            // Assert
            controller.Should().NotBeNull();
        }

        public static IEnumerable<object[]> Controllers()
            => Assembly.GetAssembly(typeof(Startup)).GetTypes()
                 .Where(type => type.IsSubclassOf(typeof(ControllerBase)))
                 .Select(x => new object[] { x });

    }
}
