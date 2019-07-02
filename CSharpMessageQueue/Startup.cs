namespace CSharpMessageQueue
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class Startup
    {
        //https://github.com/neuecc/MessagePack-CSharp#pre-code-generationunityxamarin-supports
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddCors();

            services.AddSignalR(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(10);
                options.EnableDetailedErrors = true;
            });

            services.AddSingleton(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.AllowOrigins(Configuration);

            //app.UseAuthentication();
            
            
            app.UseSignalR(routes => 
            {
                routes.MapHub<CSharpMessageHub>(Configuration.GetValue<string>("EndPoint"),
                    options => 
                    {
                        options.ApplicationMaxBufferSize = 2048 * 1024;
                    });

                routes.MapHub<JsCSharpMessageHub>($"/js{Configuration.GetValue<string>("EndPoint")}",
                    options =>
                    {
                        options.ApplicationMaxBufferSize = 2048 * 1024;
                    });
            });

            app.UseMvc();
        }
    }
}
