using Hellang.Middleware.ProblemDetails;
using System.Net;
using Traki.Api.Exceptions;
using Traki.Api.Extensions;

namespace Traki.Api.Bootstrapping
{
    public static class HttpResponseBootstrapping
    {
        public static IServiceCollection AddHttpResponseMappings(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddProblemDetails(options => options.IncludeExceptionDetails = (_, _) => configuration.IsDevelopment());

            services.AddProblemsMapping<EntityNotFoundException>(HttpStatusCode.NotFound, e => e.Message);

            return services;
        }

        public static IServiceCollection AddProblemsMapping<TException>(this IServiceCollection services, HttpStatusCode statusCode, Func<TException, string> details) where TException : Exception
        {
            services.Configure((ProblemDetailsOptions options) =>
            {
                options.Map((TException e) =>
                {
                    var problemDetails = StatusCodeProblemDetails.Create((int)statusCode);
                    problemDetails.Detail = details(e) ?? string.Empty;
                    return problemDetails;
                });
            });

            return services;
        }
    }
}
