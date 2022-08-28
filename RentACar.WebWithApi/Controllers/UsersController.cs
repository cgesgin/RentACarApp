using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;

namespace RentACar.WebWithApi.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserDto User)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = User.Email,
                    Email = User.Email
                };

                var role = new IdentityRole();
                role.Name = "User";
                await _roleManager.CreateAsync(role);

                var resault = await _userManager.CreateAsync(user, User.Password);
                if (resault.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "User");
                    return Redirect("~/Cars");
                }
                foreach (var error in resault.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserDto user)
        {
            if (ModelState.IsValid)
            {
                var identityResult = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);
                if (identityResult.Succeeded)
                {
                    return Redirect("~/Cars");
                }
                ModelState.AddModelError("", "Username or password incorrect");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
