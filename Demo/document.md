## Методы Use, Map, Run
Для конфигурации конвейера обработки запроса применяются методы Run, Map и Use.

#### Метод Run
Метод Run представляет собой простейший способ для добавления компонентов middleware в конвейер. Однако компоненты, определенные через метод Run, не вызывают никакие другие компоненты и дальше обработку запроса не передают.

#### Метод Use
Метод Use также добавляет компоненты middleware и обрабатывает запрос, но в нем может быть вызван следующий в конвейере запроса компонент middleware. Например, изменим метод Configure() следующим образом:
```
public void Configure(IApplicationBuilder app)
{
    int x = 5;
    int y = 8;
    int z = 0;
    app.Use(async (context, next) =>
    {
        z = x * y;
        await next.Invoke();
    });
 
    app.Run(async (context) =>
    {
        await context.Response.WriteAsync($"x * y = {z}");
    });
}
```
В данном случае мы используем перегрузку метода Use, которая в качестве параметров принимает контекст запроса - объект HttpContext и делегат Func<Task>, который представляет собой ссылку на следующий в конвейере компонент middleware.

Метод app.Use реализует простейшую задачу - умножение двух чисел и затем передает обработку запроса следующим компонентам middleware в конвейере.

То есть при вызове await next.Invoke() обработка запроса перейдет к тому компоненту, который установлен в методе app.Run().

#### Метод Map
Метод Map (и методы расширения MapXXX()) применяется для сопоставления пути запроса с определeнным делегатом, который будет обрабатывать запрос по этому пути. Например:
```
public void Configure(IApplicationBuilder app)
{
    app.Map("/index", Index);
    app.Map("/about", About);
 
    app.Run(async (context) =>
    {
        await context.Response.WriteAsync("Page Not Found");
    });
}
 
private static void Index(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Index");
    });
}
private static void About(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("About");
    });
}
```

пример файла конфигурации
```
{
  "color": "red",
  "namespace": { "class": { "method": "AddJson" } }
}
```
И чтобы обратиться к этой настройке, нам надо использовать знак двоеточия для обращения к иерархии настроек:
```
string text = AppConfiguration["namespace:class:method"];
```

###Объединение источников конфигурации
####При необходимости мы можем использовать сразу несколько источников конфигурации:
```
public class Startup
{
    public Startup(IHostingEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath);
        builder.AddJsonFile("conf.json");
        builder.AddEnvironmentVariables();
        builder.AddInMemoryCollection(new Dictionary<string, string>
        {
            {"firstname", "Tom"},
            {"age", "31"}
        });
 
        AppConfiguration = builder.Build();
    }
    public IConfiguration AppConfiguration { get; set; }
 
    public void ConfigureServices(IServiceCollection services)
    { }
 
    public void Configure(IApplicationBuilder app)
    {
        var color = AppConfiguration["color"];  // определен в файле conf.json
        string text = AppConfiguration["firstname"]; // определен в памяти
        app.Run(async (context) =>
        {
            await context.Response.WriteAsync($"<p style='color:{color};'>{text}</p>");
        });
    }
}
```