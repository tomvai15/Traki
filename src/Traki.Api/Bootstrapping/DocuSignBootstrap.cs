using Hellang.Middleware.ProblemDetails;
using System.Net;
using Traki.Api.Exceptions;
using Traki.Api.Services.Docusign;
using Traki.Api.Settings;

namespace Traki.Api.Bootstrapping
{
    public static class DocuSignBootstrap
    {
        public static IServiceCollection AddDocusignServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.Configure<DocuSignSettings>(configuration.GetSection(DocuSignSettings.SectionName));
            services.AddSingleton<IDocuSignService, DocuSignService>();

            return services;
        }
    }
}
