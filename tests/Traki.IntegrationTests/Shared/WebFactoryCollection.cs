using Microsoft.AspNetCore.Mvc.Testing;

namespace Traki.IntegrationTests.Shared
{
    [CollectionDefinition("Sequential")]
    public class DatabaseCollection : ICollectionFixture<WebApplicationFactory<Program>>
    {
    }
}
