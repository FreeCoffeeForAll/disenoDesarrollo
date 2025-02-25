using System.Security.Claims;
using System.Security.Principal;

namespace ToDo.Infrastructure.Extensions
{
    public static class ClaimExtensions
    {
        public static string GetClaim(this IIdentity claimsIdentity, string claimType)
        {
            var claim = 
                ((ClaimsIdentity)claimsIdentity).Claims.FirstOrDefault(s=>s.Type == claimType)?.Value;
            return claim;
        }
    }
}
