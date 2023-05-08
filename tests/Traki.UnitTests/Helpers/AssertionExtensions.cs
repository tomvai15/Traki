using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Traki.UnitTests.Helpers
{
    public static class AssertionExtensions
    {
        public static T ShouldBeOfType<T>(this ActionResult<T> response)
        {
            OkObjectResult result = (OkObjectResult)response.Result;
            result.Value.Should().BeOfType<T>();
            return (T)result.Value;
        }
    }
}
