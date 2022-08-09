using Microsoft.AspNetCore.Authorization;

namespace permissions.Filters
{
    public class PermissionRequiremnt: IAuthorizationRequirement
    {
        public string permission { get; private set; }

        public PermissionRequiremnt(string permission)
        {
            this.permission = permission;
        }
    }
}
