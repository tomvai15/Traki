using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traki.UnitTests.Fakes
{
    public static class Dummy
    {
        private static Fixture fixture = new Fixture();

        public static T Any<T>() => fixture.Create<T>();
        public static IEnumerable<T> AnyMany<T>(int count) => fixture.CreateMany<T>(count);
    }
}
