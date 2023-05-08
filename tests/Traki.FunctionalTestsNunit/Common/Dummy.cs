using AutoFixture;

namespace Traki.FunctionalTestsNunit.Helpers
{
    public static class Dummy
    {
        private static Fixture fixture = new Fixture();

        public static T Any<T>() => fixture.Create<T>();
        public static IEnumerable<T> AnyMany<T>(int count) => fixture.CreateMany<T>(count);
    }
}
