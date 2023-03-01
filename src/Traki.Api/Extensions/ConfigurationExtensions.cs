namespace Traki.Api.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsDevelopment(this IConfiguration configuration) 
            => configuration["ASPNETCORE_ENVIRONMENT"] == "Development";
    }
}
