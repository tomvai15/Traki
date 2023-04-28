using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;
using Traki.Domain.Constants;
using Traki.Domain.Settings;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace Traki.Api.Bootstrapping
{
    public static class AuthorizationBootstrap
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            var securitySettingsSection = configuration.GetSection(nameof(SecuritySettings));

            services.Configure<SecuritySettings>(securitySettingsSection);
            var securitySettings = securitySettingsSection.Get<SecuritySettings>();

            services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = "JWT_OR_COOKIE";
                        options.DefaultChallengeScheme = "JWT_OR_COOKIE";
                    })
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
                    })
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitySettings.Secret)),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    })
                    .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
                    {
                        // runs on each request
                        options.ForwardDefaultSelector = context =>
                        {
                            // filter by auth type
                            string authorization = context.Request.Headers[HeaderNames.Authorization];
                            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                                return "Bearer";

                            // otherwise always check for cookie auth
                            return "Cookies";
                        };
                    });


            var multiSchemePolicy = new AuthorizationPolicyBuilder("JWT_OR_COOKIE")
              .RequireAuthenticatedUser()
              .Build();

            services.AddAuthorization(options => { 
                options.DefaultPolicy = multiSchemePolicy;
                options.AddPolicy("ProjectManager", policy =>
                {
                    policy.RequireClaim("Role", "ProjectManager");
                });
            
            });

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
