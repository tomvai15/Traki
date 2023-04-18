using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Traki.Domain.Cryptography;
using Traki.Infrastructure.Repositories;
using Traki.Domain.Handlers;
using Traki.Domain.Repositories;
using Traki.Domain.Providers;
using Traki.Domain.Services.BlobStorage;
using Traki.Domain.Constants;

namespace Traki.Api.Bootstrapping
{
    public static class BusinessLogicBootstrap
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddTransient<IUserHandler, UserHandler>()
                .AddTransient<IDefectHandler, DefectHandler>()
                .AddTransient<IUserAuthHandler, UserAuthHandler>()
                .AddTransient<IUsersRepository, UsersHandler>()
                .AddTransient<IChecklistHandler, ChecklistHandler>()
                .AddTransient<IReportHandler, ReportHandler>()
                .AddTransient<ISectionHandler, SectionHandler>()
                .AddTransient<IProductHandler, ProductHandler>()
                .AddTransient<IRecommendationsHandler, RecommendationsHandler>()
                .AddTransient<IReportGenerator, ReportGenerator>()
                .AddTransient<IProtocolHandler, ProtocolHandler>()
                .AddTransient<IDocumentSignerHandler, DocumentSignerHandler>()
                .AddTransient<IAccessTokenProvider, AccessTokenProvider>()
                .AddTransient<IClaimsProvider, ClaimsProvider>();


            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddTransient<IUsersRepository, UsersHandler>()
                .AddTransient<ICompaniesRepository, CompaniesRepository>()
                .AddTransient<IProjectsRepository, ProjectsRepository>()
                .AddTransient<IProductsRepository, ProductsRepository>()
                .AddTransient<ISectionRepository, SectionRepository>()
                .AddTransient<IQuestionsRepository, QuestionsRepository>()
                .AddTransient<ITemplatesRepository, TemplatesRepository>()
                .AddTransient<IChecklistRepository, ChecklistRepository>()
                .AddTransient<IChecklistQuestionRepository, ChecklistQuestionRepository>()
                .AddTransient<IItemRepository, ItemRepository>()
                .AddTransient<IProtocolRepository, ProtocolRepository>()
                .AddTransient<IDrawingsRepository, DrawingsRepository>()
                .AddTransient<IDefectsRepository, DefectsRepository>()
                .AddTransient<IDefectCommentRepository, DefectCommentRepository>()
                .AddTransient<IStatusChangeRepository, StatusChangeRepository>()
                .AddTransient<ITableRepository, TableRepository>()
                .AddTransient<ITableRowRepository, TableRowRepository>();

            return services;
        }

        public static IServiceCollection AddCryptographyServices(this IServiceCollection services)
        {
            services.AddTransient<IHasherAdapter, HasherAdapter>()
                .AddTransient<IJwtTokenGenerator, JwtTokenGenerator>()
                .AddTransient<SecurityTokenHandler, JwtSecurityTokenHandler>();

            return services;
        }

        public static IServiceCollection AddWebConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WebSettings>(configuration.GetSection(WebSettings.SectionName));

            return services;
        }
    }
}
