using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Plutus.Api.IntegrationTests
{
    public sealed class FakePolicyEvaluator : IPolicyEvaluator
    {
        public async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            const string testScheme = "FakeScheme";
            var principal = new ClaimsPrincipal();
            principal.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Administrator"),
                new Claim(ClaimTypes.NameIdentifier, "sanjay")
            }, testScheme));
            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal,
                new AuthenticationProperties(), testScheme)));
        }

        Task<PolicyAuthorizationResult> IPolicyEvaluator.AuthorizeAsync(AuthorizationPolicy policy,
            AuthenticateResult authenticationResult, HttpContext context,
            object? resource)
        {
            return AuthorizeAsync();
        }

        private async Task<PolicyAuthorizationResult> AuthorizeAsync()
        {
            return await Task.FromResult(PolicyAuthorizationResult.Success());
        }
    }
}