using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace TagAC.BuildingBlocks.Authorization
{
    public class RoleClaimRequisiteFilter : IAuthorizationFilter
    {
        private readonly string _roleName;

        public static readonly string RoleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        public RoleClaimRequisiteFilter(string roleName)
        {
            _roleName = roleName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            if (!CustomClaimsAuthorize.ValidateUserClaims(context.HttpContext, RoleClaim, _roleName))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
