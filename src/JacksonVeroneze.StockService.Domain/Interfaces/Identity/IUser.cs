using System.Collections.Generic;
using System.Security.Claims;

namespace JacksonVeroneze.StockService.Domain.Interfaces.Identity
{
    public interface IUser
    {
        string Name { get; }

        bool IsAuthenticated();

        IEnumerable<Claim> GetClaimsIdentity();
    }
}
