namespace CSharpMessageQueue
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;

    public static class CSharpAllowOrigins
    {
        public static IApplicationBuilder AllowOrigins(this IApplicationBuilder app, IConfiguration configuration)
        {
            string allowOrigins = configuration.GetValue<string>("Origins");

            app.UseCors(builder => builder
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins(allowOrigins.Split(","))
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            return app;
        }
    }
}
