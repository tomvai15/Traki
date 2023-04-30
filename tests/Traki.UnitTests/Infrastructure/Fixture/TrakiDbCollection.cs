using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traki.UnitTests.Infrastructure.Fixture
{
    [CollectionDefinition("Sequential")]
    public class TrakiDbCollection : ICollectionFixture<TrakiDbFixture>
    {
    }
}
