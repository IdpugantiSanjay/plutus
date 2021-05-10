using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Plutus.Api.Middleware
{
    public class AddUserToRouteMiddleware
    {
        private readonly RequestDelegate _next;

        public AddUserToRouteMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
        {
            context.Request.RouteValues.Add("Username", context.User?.Identity?.Name);
            await _next(context);
        }
    }
}