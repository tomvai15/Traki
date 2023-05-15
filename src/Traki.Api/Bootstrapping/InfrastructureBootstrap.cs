using Traki.Domain.Services.BlobStorage;
using Traki.Domain.Services.Docusign;
using Traki.Domain.Services.Email;
using Traki.Domain.Services.Notifications;
using Traki.Infrastructure.Services;

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

        public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));
            services.AddSingleton<IEmailService, EmailService>();

            return services;
        }

        public static IServiceCollection AddNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<INotificationService, NotificationService>();
            services.Configure<NotificationSettings>(configuration.GetSection(NotificationSettings.SectionName));

            return services;
        }
    }
}
