﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DiscoveringAsp.netCore.Middlewares
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
       

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async  Task Invoke (HttpContext httpContext)
        {

             await _next.Invoke(httpContext);
        }
    }
}
