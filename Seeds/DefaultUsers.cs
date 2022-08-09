using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace permissions.Constant
{
    public static class DefaultUsers
    {
        public static async Task SeedsBasicAsync(UserManager<IdentityUser> userManager)
        {
            IdentityUser newUser = new IdentityUser
            {
                UserName = "Basic@user",
                Email = "Basic@user",
                EmailConfirmed = true,
            };

            var user = await userManager.FindByEmailAsync(newUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(newUser, "P@ssword123");
                await userManager.AddToRoleAsync(newUser, Roles.Basic.ToString());
            }
        }

        public static async Task SeedsSuperManager(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var newUser = new IdentityUser
            {
                UserName = "SuperAdmin@user",
                Email = "SuperAdmin@user",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(newUser.Email);

            if(user == null)
            {
                await userManager.CreateAsync(newUser, "P@ssword123");
                await userManager.AddToRolesAsync(newUser, new List<string>{ Roles.Admin.ToString(), Roles.SuperAdmin.ToString(), Roles.Basic.ToString()});
            }

            await roleManager.SeedsClaimForSuperManager();
        }
        
        public static async Task SeedsClaimForSuperManager(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());

            await roleManager.AddPermissionClaims(adminRole, "addUsers");
        }

        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsList(module);

            foreach (var permission in allPermissions)
            {
                if(!allClaims.Any(n => n.Type == "Permission" && n.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }
}
