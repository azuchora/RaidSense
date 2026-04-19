using System;
using RaidSense.Server.Exceptions.Http;

namespace RaidSense.Server.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseHttpException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
        }
    }
}
