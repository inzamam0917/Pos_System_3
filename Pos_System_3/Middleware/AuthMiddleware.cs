using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class  AuthMiddleware
{
    private readonly RequestDelegate _next;
    private const string AuthKey = "AuthKey";
    private const string AuthValue = "12345678";

    public  AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(AuthKey, out var extractedAuthKey) || extractedAuthKey != AuthValue)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await _next(context);
    }
}
