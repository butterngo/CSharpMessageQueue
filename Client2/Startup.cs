namespace Client2
{
    using CSharpMessageQueueClient;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using System.Text;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.RegisterCSharpClient("http://localhost:7500/c-sharp-message-queue", "client2");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.StartConnection().OnHandleReceivedMessage += (message) =>
            {
                var test = Encoding.UTF8.GetString(message.Body);
            };

            app.UseMvc();
        }
    }
}
