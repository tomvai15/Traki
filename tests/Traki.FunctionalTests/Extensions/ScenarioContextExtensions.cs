using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace Traki.FunctionalTests.Extensions
{
    public static class ScenarioContextExtensions
    {
        public static T GetRequiredService<T>(this ScenarioContext scenarioContext) where T: notnull
        {
            var serviceScope = scenarioContext.Get<IServiceScope>();

            if (serviceScope == null)
            {
                throw new ArgumentNullException("IServiceScope was not registered");
            }
            return serviceScope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
