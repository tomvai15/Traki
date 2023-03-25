using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Traki.Domain.Cryptography;
using Traki.Infrastructure.Repositories;
using Traki.Domain.Handlers;
using Traki.Domain.Repositories;

namespace Traki.Api.Bootstrapping
{
    public static class BusinessLogicBootstrap
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddTransient<IUserAuthHandler, UserAuthHandler>()
                .AddTransient<IUsersRepository, UsersHandler>()
                .AddTransient<IChecklistHandler, ChecklistHandler>()
                .AddTransient<IReportHandler, ReportHandler>()
                .AddTransient<ISectionHandler, SectionHandler>();

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
                .AddTransient<IChecklistQuestionRepository, ChecklistQuestionRepository>();

            return services;
        }

        public static IServiceCollection AddCryptographyServices(this IServiceCollection services)
        {
            services.AddTransient<IHasherAdapter, HasherAdapter>()
                .AddTransient<IJwtTokenGenerator, JwtTokenGenerator>()
                .AddTransient<SecurityTokenHandler, JwtSecurityTokenHandler>();

            return services;
        }
    }
}
