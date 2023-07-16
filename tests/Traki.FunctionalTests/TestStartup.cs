using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Traki.FunctionalTests.Bootstrapping;

namespace Traki.FunctionalTests
{
    [Binding]
    public class TestStartup
    {
        [BeforeTestRun]
        public static void ConfigureServices()
        {
            var configuration = Accessor.Configuration;
            var services = Accessor.ServiceCollection;
            services.ConfigureServices(configuration);
        }

        [BeforeScenario]
        public void CreateScope(ScenarioContext scenarioContext)
        {
            var serviceProvider = Accessor.ServiceProvider;
            var scope = serviceProvider.CreateScope();

            scenarioContext.Set(scope);
        }

        [AfterScenario]
        public void DisposeScope(ScenarioContext scenarioContext)
        {
            var scope = scenarioContext.Get<IServiceScope>();
            scope.Dispose();
        }
    }
}
