using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Traki.Api.Cryptography;
using Traki.Api.Handlers;
using Traki.Api.Repositories;

namespace Traki.Api.Bootstrapping
{
    public static class BusinessLogicBootstrap
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserAuthHandler, UserAuthHandler>()
                .AddTransient<IUsersHandler, UsersHandler>()
                .AddTransient<IProjectsHandler, ProjectsHandler>()
                .AddTransient<IProductsHandler, ProductsHandler>()
                .AddTransient<IQuestionsHandler, QuestionsHandler>()
                .AddTransient<ITemplatesHandler, TemplatesHandler>()
                .AddTransient<IChecklistRepository, ChecklistRepository>()
                .AddTransient<IChecklistQuestionHandler, ChecklistQuestionHandler>()
                .AddTransient<IChecklistHandler, ChecklistHandler>();

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
