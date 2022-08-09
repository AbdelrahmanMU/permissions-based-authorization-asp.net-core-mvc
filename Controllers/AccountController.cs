//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using permissions.View_Model;
using System.Net;
using System.Security.Claims;

namespace permissions.Controllers
{
    public class AccountController : Controller
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> login(LoginUserViewModel userVM)
        {
            if(ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(userVM.userName);

                if (user != null)
                {
                    bool pass = await userManager.CheckPasswordAsync(user, userVM.password);

                    if (pass)
                    {
                        await signInManager.SignInAsync(user, userVM.rememberMe);
                        return RedirectToAction("Index", "Users");
                    }
                }

                ModelState.AddModelError("", "userName or Password incorrect");

            }

            return View(userVM);
        }


        public async Task<IActionResult> logout()
        {
            await signInManager.SignOutAsync();
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //await AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return View("login");
        }
    }
}
