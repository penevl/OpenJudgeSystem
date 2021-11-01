namespace OJS.Servers.Ui.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OJS.Data.Models.Users;
    using OJS.Servers.Ui.Models;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Authorize]
    public class AccountController : BaseViewController
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly SignInManager<UserProfile> signInManager;

        public AccountController(
            UserManager<UserProfile> userManager,
            SignInManager<UserProfile> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var signInResult = await this.signInManager
                .PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            if (!signInResult.Succeeded)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    "Invalid username or password");

                return this.View(model);
            }

            var user = await this.userManager.FindByNameAsync(model.UserName);

            var roles = await this.userManager.GetRolesAsync(user);

            var claims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                // ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return this.Redirect(returnUrl);
        }
    }
}