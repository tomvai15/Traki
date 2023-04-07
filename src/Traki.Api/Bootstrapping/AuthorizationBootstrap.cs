﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Traki.Domain.Constants;
using Traki.Domain.Settings;

namespace Traki.Api.Bootstrapping
{
    public static class AuthorizationBootstrap
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            var securitySettingsSection = configuration.GetSection(nameof(SecuritySettings));

            services.Configure<SecuritySettings>(securitySettingsSection);
            var securitySettings = securitySettingsSection.Get<SecuritySettings>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
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
            var multiSchemePolicy = new AuthorizationPolicyBuilder(
                CookieAuthenticationDefaults.AuthenticationScheme,
                JwtBearerDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser()
              .Build();

            services.AddAuthorization(options => { 
                options.DefaultPolicy = multiSchemePolicy;
                options.AddPolicy("ProjectManager", policy =>
                {
                    policy.RequireClaim("Role", "ProjectManager");
                });
            
            });*/

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
