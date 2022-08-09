using Microsoft.AspNetCore.Authorization;

namespace permissions.Filters
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequiremnt>
    {
        public PermissionAuthorizationHandler()
        {

        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequiremnt requirement)
        {
            if (context.User == null)
                return;

            var canAccess = context.User.Claims.Any(c => c.Type == "Permission" && c.Value == requirement.permission && c.Issuer == "LOCAL AUTHORITY");
            
            if (canAccess)
            {
                context.Succeed(requirement);
                return;
            }

        }
    }
}
