using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using permissions.Constant;
using permissions.View_Model;
using System.Security.Claims;

namespace permissions.Controllers
{
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public IActionResult Index()
        {
            return View(roleManager.Roles.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> add(AddRoleViewModel2 roleVM)
        {
            if (ModelState.IsValid)
            {
                if (!await roleManager.RoleExistsAsync(roleVM.name))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleVM.name.Trim()));
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("name", "Role Already Exist");
            }
            return View("Index", roleManager.Roles.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> managePermission(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
                return NotFound();

            var roleClaims = await roleManager.GetClaimsAsync(role);
            var allClaims = Permissions.GenerateAllPermissions();

            var allPermissions = allClaims.Select(p => new CheckBoxViewModel { displayName = p }).ToList();

            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(p => p.Value == permission.displayName))
                    permission.isSelected = true;
            }

            PermissionsFormViewModel model = new PermissionsFormViewModel
            {
                id = roleId,
                roleName = role.Name,
                roleClaims = allPermissions
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> managePermission(PermissionsFormViewModel permissionVM)
        {
            var role = await roleManager.FindByIdAsync(permissionVM.id);

            if (role == null)
                return NotFound();
            
            var roleClaims = await roleManager.GetClaimsAsync(role);
            var allClaims = Permissions.GenerateAllPermissions();

            foreach (var permission in permissionVM.roleClaims)
            {
                if(!roleClaims.Any(c => c.Value == permission.displayName) && permission.isSelected)
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission.displayName));

                if (roleClaims.Any(c => c.Value == permission.displayName) && !permission.isSelected)
                    await roleManager.RemoveClaimAsync(role, new Claim("Permission", permission.displayName));
            }

            return RedirectToAction("Index");
        }
    }
}
