using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace JacksonVeroneze.StockService.Api.Util.Authentication
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            HasScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            string[] scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer)?.Value
                .Split(' ');

            if ((scopes ?? Array.Empty<string>()).Any(s => s == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
