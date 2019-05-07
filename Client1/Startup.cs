namespace Client1
{
    using CSharpEventBus;
    using CSharpMessageQueueClient;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.RegisterCSharpClient("http://localhost:7500/c-sharp-message-queue", "client1");

            services.AddSingleton<IEventBus, CSharpEventBusHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.StartConnection();

            app.UseMvc();
        }
    }
}
