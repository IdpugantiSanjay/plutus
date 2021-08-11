using System.Net;

namespace Plutus.Api.Middleware;

public class CheckIfUsernameInTokenIsSameAsRequestUsername
{
    private readonly RequestDelegate _next;

    public CheckIfUsernameInTokenIsSameAsRequestUsername(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.RouteValues.ContainsKey("Username"))
        {
            var username = context.Request.RouteValues["Username"]?.ToString();
            var tokenNameIdentifier = context.User.Identity?.Name;

            if (username != tokenNameIdentifier)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                { Message = $"Token is not generated for {username}" }));
            }
        }
        await _next(context);
    }
}
