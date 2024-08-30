using AutoFixture;
using AutoFixture.Kernel;
using System.Reflection;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using MethodInvoker = AutoFixture.Kernel.MethodInvoker;

namespace Traki.UnitTests.Helpers
{
    public static class Dummy
    {
        private static IFixture fixture = new Fixture();

        public static T Any<T>() => fixture.Create<T>();
        public static IEnumerable<T> AnyMany<T>(int count = 3) => fixture.CreateMany<T>(count);
    }

    public class IgnoreObjectsCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new MethodInvoker(
                        new ModestConstructorQuery()),
                    new ObjectPropertySpecification()));
        }

        private class ObjectPropertySpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                var pi = request as PropertyInfo;

                if (pi == null)
                {
                   return true;
                }

                if (pi.PropertyType == typeof(User))
                {
                    return true;
                }

                if (pi.PropertyType == typeof(Defect))
                {
                    return true;
                }

                return false;
            }
        }
    }
}
