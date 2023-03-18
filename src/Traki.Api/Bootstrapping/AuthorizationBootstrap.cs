using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Traki.Api.Constants;
using Traki.Api.Settings;

namespace Traki.Api.Bootstrapping
{
    public static class AuthorizationBootstrap
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            WebSettings webSettings  = configuration.GetSection(nameof(WebSettings)).Get<WebSettings>();

            var securitySettingsSection = configuration.GetSection(nameof(SecuritySettings));

            services.Configure<SecuritySettings>(securitySettingsSection);
            var securitySettings = securitySettingsSection.Get<SecuritySettings>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Events.OnRedirectToLogin = (context) =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToAccessDenied = (context) =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    };
                });
            /*
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
               });*/

            services.AddAuthorization(options => {
                options.AddPolicy("admin", policy => policy.RequireRole("admin"));
                }
            );

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(Policy.DevelopmentCors, builder =>
                {
                    builder.WithOrigins("http://localhost:3000/")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .SetIsOriginAllowed((_) => true);
                });
            });

            return services;
        }
    }
}
