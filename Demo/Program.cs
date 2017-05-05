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
                .UseContentRoot(Directory.GetCurrentDirectory())//определяет каталог содержимого приложения
                .UseIISIntegration()//Этот метод обеспечивает интеграцию приложения с веб-сервером IIS, через который по умолчанию перенаправляются запросы на сервер Kestrel. 
                 //Однако также необязательно использовать этот метод, если мы не используем IIS.
                .UseStartup<Startup>()//Этим вызовом устанавливается стартовый класс приложения - класс Startup
                .UseApplicationInsights()
                .Build();//метод Build(), который собственно создает хост - объект IWebHost, в рамках которого развертывается веб-приложение.

            host.Run();//В самом конце запускается приложение:
        }
    }
}
