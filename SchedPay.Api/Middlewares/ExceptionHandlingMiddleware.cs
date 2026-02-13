using System.Net;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace SchedPay.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception");

            var problem = new ProblemDetails
            {
                Title = "Unexpected error",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = "An unexpected error occurred.",
                Instance = context.Request.Path
            };

            context.Response.StatusCode = problem.Status.Value;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
