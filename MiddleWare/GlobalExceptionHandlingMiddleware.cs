using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.MiddleWay
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
   
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)=>
            _logger = logger;
        

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new() 
                { 
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title= e.Message,
                    Detail = "An internal server has occured"
                };

                string json = JsonSerializer.Serialize(problem);

                await context.Response.WriteAsync(json);

                context.Response.ContentType= "application/json";
            }
        }
    }
}
