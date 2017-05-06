using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    //Данный класс определяет один метод GetTime(), который возвращает текущее время.
    public class TimeService
    {
        public string GetTime() => DateTime.Now.ToString("hh:mm:ss");
    }

    public static partial class ServiceProviderExtensions
    {
        //Нередко для сервисов создают собственные методы добавления в виде методов расширения для интерфейса IServiceCollection. Например:
        public static void AddTimeService(this IServiceCollection services)
        {
            services.AddTransient<TimeService>();
        }
    }
}
