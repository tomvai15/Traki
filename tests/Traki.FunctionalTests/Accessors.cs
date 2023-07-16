using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Traki.FunctionalTests
{
    public class Accessor
    {
        public static IServiceCollection ServiceCollection = new ServiceCollection();

        private static Lazy<IConfiguration> _configuration = new Lazy<IConfiguration>(CreateConfiguration);
        private static Lazy<IServiceProvider> _serviceProvider = new Lazy<IServiceProvider>(ServiceCollection.BuildServiceProvider());

        public static IConfiguration Configuration => _configuration.Value;
        public static IServiceProvider ServiceProvider => _serviceProvider.Value;

        private static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .Build();
        }
    }
}
