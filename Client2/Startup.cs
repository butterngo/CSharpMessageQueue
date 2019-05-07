namespace Client2
{
    using Client2.Events;
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

            services.RegisterCSharpClient("http://localhost:7500/c-sharp-message-queue", "client2");

            services.AddSingleton<IEventBus, CSharpEventBusHandler>();

            services.AddTransient<IIntegrationEventHandler<UserEvent>, CSharpEventHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            SubscribeEvents(app);

            app.UseMvc();
        }

        public void SubscribeEvents(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices
             .GetRequiredService<IEventBus>();

            eventBus.Subscribe(app.ApplicationServices.GetService<IIntegrationEventHandler<UserEvent>>());

            app.StartConnection();
        }
    }
}
