using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace TagAC.BuildingBlocks.Authorization
{
    public class CustomClaimsAuthorize
    {
        public static bool ValidateUserClaims(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated && context.User.Claims.Any(x => x.Type == claimName && x.Value.Split(',').Contains(claimValue));
        }
    }
}
