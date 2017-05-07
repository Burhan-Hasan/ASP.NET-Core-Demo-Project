using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Services;

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
            /*
             Метод ConfigureServices() управляет добавлением сервисов в приложение. После добавления сервисы можно получить и использовать в любой части приложения.
             Сам термин "сервис" в данном случае может представлять любой объект, функциональность которого может использоваться в приложении.
            */
            services.AddMvc();

            /*
             Все сервисы приложения помещаются в коллекцию IServiceCollection, которая передается в качестве параметра в метод ConfigureServices(). 
             Для добавления сервиса в эту коллекцию применяется метод AddTransient():
            */

            //Если сервис добавляется в методе ConfigureServices(), то мы сможем получить его через Dependency Injection
            //конструктор: public TimerMiddleware(RequestDelegate next, TimeService timeService)
            services.AddTransient<TimeService>();
        }

        private static void Developer(IApplicationBuilder app)
        {
            /*
             После добавления сервиса мы его можем получить и использовать в любой части приложения. 
             В частности, мы его можем получить с помощью метода IApplicationBuilder.ApplicationServices.GetService:
            */
            app.Run(async (context) =>
            {
                /*
                Причем объект IApplicationBuilder сам представляет собой сервис, только он добавляется по умолчанию самой системой. 
                Так, если мы посмотрим в режиме отладки на коллекцию IServiceCollection, то сможем там увидеть порядка полтора десятка сервисов: 
                */
                TimeService timeService = app.ApplicationServices.GetService<TimeService>();
                await context.Response.WriteAsync($"Burhan Hasan Time:{timeService.GetTime()}");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            /*
             Oбъект IApplicationBuilder сам представляет собой сервис, только он добавляется по умолчанию самой системой. 
             Так, если мы посмотрим в режиме отладки на коллекцию IServiceCollection, то сможем там увидеть порядка полтора десятка сервисов
             */
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

            app.UseToken("5454846");

            app.Map("/Developer", Developer);

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
