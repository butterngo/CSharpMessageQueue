namespace CSharpMessageQueueClient
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class CSharpExtension
    {
        public static IServiceCollection RegisterCSharpClient(this IServiceCollection services,
            string host,
            string uniqueKey)
        {
            var signalrClientConnectionFactory = new CSharpClientConnectionFactory(host, uniqueKey);

            services.AddSingleton<ICSharpClientConnectionFactory>(signalrClientConnectionFactory);

            return services;
        }

        public static ICSharpClientConnectionFactory StartConnection(this IApplicationBuilder app)
        {
            var signalr = app.ApplicationServices
                .GetRequiredService<ICSharpClientConnectionFactory>();

            signalr.StartConnection();

            return signalr;
        }
    }
}
