using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo
{
    public class TokenMiddleware
    {
        readonly RequestDelegate _next;

        string _pattern;

        public TokenMiddleware(RequestDelegate next, string pattern)
        {
            _next = next;
            _pattern = pattern;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Query["token"].ToString();
            if (string.IsNullOrEmpty(token) || token != _pattern)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Invalid token");
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }

    public static partial class Extensions
    {
        public static IApplicationBuilder UseToken(this IApplicationBuilder builder, string pattern)
        {
            /*
              С помощью метода UseMiddleware<T> в конструктор объекта 
              TokenMiddleware будет внедряться объект для параметра RequestDelegate next. 
              Поэтому явным образом передавать значение для этого параметра нам не нужно.
             */
            return builder.UseMiddleware<TokenMiddleware>(pattern);
        }
    }
}
