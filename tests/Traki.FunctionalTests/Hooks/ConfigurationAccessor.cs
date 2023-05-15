using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traki.FunctionalTests.Hooks
{
    public static class ConfigurationAccessor
    {
        public static readonly Lazy<IConfigurationRoot> LazyConfiguration = 
            new (GetConfigurationRoot, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IConfigurationRoot Configuration => LazyConfiguration.Value;

        private static IConfigurationRoot GetConfigurationRoot()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        public static string WebUrl =>  Configuration.GetSection("WebSettings:Url").Value;
    }
}
