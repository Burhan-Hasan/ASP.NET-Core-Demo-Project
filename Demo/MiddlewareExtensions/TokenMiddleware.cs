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

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Query["token"].ToString();
            if (string.IsNullOrEmpty(token) || token != "123456")
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
        public static IApplicationBuilder UseToken(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenMiddleware>();
        }
    }
}
