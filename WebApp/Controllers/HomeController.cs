using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApp
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<HomeController> logger;
        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager)
        {
            this.logger = logger;
            this.signinManager = signinManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Site here to view amazing photographies like the one here and sign up to upload different premium photographies and become a part of our great community.";

            return View();
        }

        public IActionResult Login(string returnUrl)
        {


            return View(new User()
            {
                ReturnUrl = returnUrl
            });
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user){
            if (ModelState.IsValid)
            {
                var getUser = await userManager.FindByNameAsync(user.UserName);
                if (getUser != null)
                {
                    var result = await signinManager.PasswordSignInAsync(getUser, user.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (string.IsNullOrEmpty(user.ReturnUrl))
                            return RedirectToAction("Index", "Home");
                        return RedirectToAction(user.ReturnUrl);
                    }
                }
            }
            ModelState.AddModelError("LoginError", "Username/Password not found");
            return View(user);
        }

        public IActionResult Register(){
            return View();
        }

        public async Task CreateRoles(){
            bool exist = await roleManager.RoleExistsAsync("ADMIN");
            if(!exist){
                var role = new IdentityRole
                {
                    Name = "ADMIN"
                };
                await roleManager.CreateAsync(role);
            }

            exist = await roleManager.RoleExistsAsync("USER");
            if (!exist)
            {
                var role = new IdentityRole
                {
                    Name = "USER"
                };
                await roleManager.CreateAsync(role);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user){
            if (ModelState.IsValid)
            {
                await CreateRoles();
                var registerUser = new ApplicationUser() { UserName = user.UserName, Email = user.Email };
                var result = await userManager.CreateAsync(registerUser, user.Password);
                await userManager.AddToRoleAsync(registerUser, "USER");
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                logger.LogError(errors.ToString());
            }
            return View(user);

        }
        [Authorize]
        public async Task<IActionResult> Logout(){
            await signinManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
