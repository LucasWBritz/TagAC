using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TagAC.BuildingBlocks.Authorization
{
    public class RoleAuthorizationAttribute : TypeFilterAttribute
    {
        public RoleAuthorizationAttribute(string roleName) : base(typeof(RoleClaimRequisiteFilter))
        {
            Arguments = new object[] { roleName };
        }
    }
}
