using System.Collections.Generic;
using System.Security.Claims;
using JacksonVeroneze.StockService.Domain.Interfaces.Identity;
using Microsoft.AspNetCore.Http;

namespace JacksonVeroneze.StockService.Infra.Identity
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
            => _accessor = accessor;

        public string Name
            => _accessor?.HttpContext?.User?.Identity?.Name;

        public bool IsAuthenticated()
            => _accessor?.HttpContext?.User?.Identity != null
               && _accessor.HttpContext.User.Identity.IsAuthenticated;

        public IEnumerable<Claim> GetClaimsIdentity()
            => _accessor?.HttpContext?.User?.Claims;
    }
}
