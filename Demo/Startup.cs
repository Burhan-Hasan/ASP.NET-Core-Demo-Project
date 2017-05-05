using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo
{
    public class Startup
    {
        /*
         Класс Startup является входной точкой в приложение ASP.NET Core. 
         Этот класс производит конфигурацию приложения, 
         настраивает сервисы, которые приложение будет использовать, 
         устанавливает компоненты для обработки запроса или middleware.
        */
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //IHostingEnvironment: позволяет взаимодействовать со средой, в которой запускается приложение

            //ILoggerFactory: предоставляет механизм логгирования в приложении
            /*
             Метод Configure устанавливает, как приложение будет обрабатывать запрос. 
             Для установки компонентов, которые обрабатывают запрос, используются методы объекта IApplicationBuilder. 
             Объект IApplicationBuilder является обязательным параметром для метода Configure.
             */

            // установка консоли для вывода лога
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // если приложение в процессе разработки
            if (env.IsDevelopment())
            {
                // то выводим информацию об ошибке, при наличии ошибки
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                // установка обработчика ошибок
                app.UseExceptionHandler("/Home/Error");
            }
            // установка обработчика статических файлов
            app.UseStaticFiles();

            // Установка компонентов MVC для обработки запроса
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            /*
             Причем важно, что чтобы использовать некоторые сервисы, их вначале надо зарегистрировать. 
             Так, мы не смогли бы использовать метод app.UseMvc() в методе Configure, 
             если бы мы не использовали бы вызов services.AddMvc() в методе ConfigureServices().
             */
        }
    }
}
