using Microsoft.AspNetCore.Identity;
using permissions.Constant;

namespace permissions.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedsAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
            }
        }  
    }
}
