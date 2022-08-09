using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using permissions.View_Model;

namespace permissions.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        UserManager<IdentityUser> userManager;
        RoleManager<IdentityRole> roleManager;
        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            List<IdentityUser> users = userManager.Users.ToList();
            List<UsersRolesViewModel> usersVM = new List<UsersRolesViewModel>();

            foreach (var user in users)
            {
                UsersRolesViewModel userVM = new UsersRolesViewModel();

                userVM.id = user.Id;
                userVM.userName = user.UserName;
                userVM.email = user.Email;
                userVM.roles = userManager.GetRolesAsync(user).Result;

                usersVM.Add(userVM);
            }

            return View(usersVM);
        }

        public async Task<IActionResult> manageRoles(string userId)
        {
            IdentityUser user = await userManager.FindByIdAsync(userId);

            if (user == null) 
                return NotFound();

            List<IdentityRole> roles = roleManager.Roles.ToList();

            UserRoleViewModel userVM = new UserRoleViewModel();
            userVM.id = user.Id;
            userVM.userName = user.UserName;
            userVM.roles = roles.Select(role => new CheckBoxViewModel
            {
                displayName = role.Name,
                isSelected = userManager.IsInRoleAsync(user, role.Name).Result
            }).ToList();

            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> updateRoles(UserRoleViewModel userVM)
        {
            IdentityUser user = await userManager.FindByIdAsync(userVM.id);

            if (user == null)
                return NotFound();

            var roles = await userManager.GetRolesAsync(user);

            foreach(var role in userVM.roles)
            {
                if(!roles.Any(n => n == role.displayName) && role.isSelected)
                {
                    await userManager.AddToRoleAsync(user, role.displayName);
                }
                if(roles.Any(n => n == role.displayName) && !role.isSelected)
                {
                    await userManager.RemoveFromRoleAsync(user, role.displayName);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult NewUser()
        {
            var roles = roleManager.Roles.ToList();
            List<AddRoleViewModel> rolesList = new List<AddRoleViewModel>();

            foreach(var role in roles)
            {
                AddRoleViewModel roleVM = new AddRoleViewModel();

                roleVM.id = role.Id;
                roleVM.name = role.Name;

                rolesList.Add(roleVM);
            }

            return View(rolesList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewUser(AddUserViewModel newUser)
        {
            if(ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = newUser.userName;
                user.PasswordHash = newUser.password;
                user.Email = newUser.email;

                var result = await userManager.CreateAsync(user, newUser.password);

                if(result.Succeeded)
                {
                    var result2 = await userManager.AddToRoleAsync(user, newUser.role);

                    if(result2.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    foreach(var err in result2.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View(newUser);
        }


    }
}
