using Hellang.Middleware.ProblemDetails;
using System.Net;
using Traki.Api.Exceptions;
using Traki.Api.Services.BlobStorage;
using Traki.Api.Services.Docusign;
using Traki.Api.Settings;

namespace Traki.Api.Bootstrapping
{
    public static class InfrastructureBootstrap
    {
        public static IServiceCollection AddDocusignServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.Configure<DocuSignSettings>(configuration.GetSection(DocuSignSettings.SectionName));
            services.AddSingleton<IDocuSignService, DocuSignService>();

            return services;
        }

        public static IServiceCollection AddBlobStorageServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlobStorageSettings>(configuration.GetSection(BlobStorageSettings.SectionName));
            services.AddSingleton<IStorageService, BlobStorageService>();

            return services;
        }
    }
}
