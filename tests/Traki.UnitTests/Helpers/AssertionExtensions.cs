using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Api.Contracts.Auth;

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
