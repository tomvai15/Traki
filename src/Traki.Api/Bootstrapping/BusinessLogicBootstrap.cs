using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Traki.Api.Cryptography;
using Traki.Api.Data.Repositories;
using Traki.Api.Handlers;

namespace Traki.Api.Bootstrapping
{
    public static class BusinessLogicBootstrap
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserAuthHandler, UserAuthHandler>()
                .AddTransient<IUsersRepository, UsersHandler>()
                .AddTransient<IProjectsRepository, ProjectsRepository>()
                .AddTransient<IProductsRepository, ProductsRepository>()
                .AddTransient<IQuestionsRepository, QuestionsRepository>()
                .AddTransient<ITemplatesRepository, TemplatesRepository>()
                .AddTransient<IChecklistRepository, ChecklistRepository>()
                .AddTransient<IChecklistQuestionRepository, ChecklistQuestionRepository>()
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
