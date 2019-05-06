namespace Client2
{
    using CSharpMessageQueueClient;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using System.Text;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.RegisterCSharpClient("http://localhost:7500/c-sharp-message-queue", "client2");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
