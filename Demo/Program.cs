using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
             В методе Main для создания хоста веб-приложения используется 
             класс WebHostBuilder из пространства имен Microsoft.AspNetCore.Hosting. 
             С помощью последовательного вызова цепочки методов у 
             WebHostBuilder инициализирует веб-сервер для развертывания веб-приложения.
             */
            var host = new WebHostBuilder()
                .UseKestrel() //устанавливает в качестве веб-сервера Kestrel. Хотя необязательно использовать именно Kestrel.
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
