using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Traki.Api.Constants;
using Traki.Api.Settings;

namespace Traki.Api.Bootstrapping
{
    public static class AuthorizationBootstrap
    {
        public static IServiceCollection AddAuthorisationServices(this IServiceCollection services, IConfiguration configuration)
        {
            WebSettings webSettings  = configuration.GetSection(nameof(WebSettings)).Get<WebSettings>();

            services.AddCors(options =>
            {
                options.AddPolicy(Policy.DevelopmentCors, builder =>
                {
                    builder.WithOrigins(webSettings.Url)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .SetIsOriginAllowed((x) => true)
                           .AllowCredentials();
                });
            });

            var securitySettingsSection = configuration.GetSection(nameof(SecuritySettings));

            services.Configure<SecuritySettings>(securitySettingsSection);
            var securitySettings = securitySettingsSection.Get<SecuritySettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitySettings.Secret)),
                        ValidateIssuer = false, 
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(Policy.DevelopmentCors, builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .SetIsOriginAllowed((_) => true);
                });
            });

            return services;
        }
    }
}
