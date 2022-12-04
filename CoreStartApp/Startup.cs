using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CoreStartApp
{
    public class Startup   
    { 
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                // Для логирования данных о запросе используем свойства объекта HttpContext
                Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
                });
            });

            // Все прочие страницы имеют отдельные обработчики
            app.Map("/about", About);
            app.Map("/config", Config);

            // Обработчик для ошибки "страница не найдена"
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Page not found");
            });

            /// <summary>
            ///  Обработчик для страницы About
            /// </summary>
            void About(IApplicationBuilder app)
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync($"{env.ApplicationName} - ASP.Net Core tutorial project");
                });
            }

            /// <summary>
            ///  Обработчик для главной страницы
            /// </summary>
            void Config(IApplicationBuilder app)
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync($"App name: {env.ApplicationName}. App running configuration: {env.EnvironmentName}");
                });
            }
        }
    }
}
