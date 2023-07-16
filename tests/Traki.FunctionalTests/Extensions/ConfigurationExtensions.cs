using Microsoft.Extensions.Configuration;

namespace Traki.FunctionalTests.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool LaunchBrowser(this IConfiguration configuration)
        {
            bool.TryParse(configuration.GetSection("LaunchBrowser").Value, out bool launchBrowser);
            return launchBrowser;
        }

        public static string WebUrl(this IConfiguration configuration) => configuration.GetSection("WebSettings:Url").Value;      
    }
}
