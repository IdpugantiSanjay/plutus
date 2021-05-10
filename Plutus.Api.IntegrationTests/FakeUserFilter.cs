using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Web;
using Plutus.Api.Common;

namespace Plutus.Api.IntegrationTests
{
    public class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "sanjay"),
                new(ClaimTypes.Name, "Sanjay Idpuganti"),
                new(ClaimTypes.Email, "sanjay11@outlook.com"),
                new(ClaimConstants.Scp, Scopes.UserRead)
            }));

            await next();
        }
    }
}