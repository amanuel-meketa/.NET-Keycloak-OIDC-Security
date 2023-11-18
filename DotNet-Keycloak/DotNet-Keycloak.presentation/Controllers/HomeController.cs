using DotNet_Keycloak.business.Contract;
using DotNet_Keycloak.business.Model;
using DotNet_Keycloak.presentation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_Keycloak.presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;

            var userClaims = _userService.GetUserClaims()
                .Select(claim => new UserClaimsModel { Type = claim.Type, Value = claim.Value })
                .ToList();

            userClaims.Add(new UserClaimsModel { Type = "access_token", Value = accessToken });

            return View(userClaims);
        }
        [HttpPost]
        public IActionResult Logout()
        {
            // Clear authentication cookies
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            // Redirect to Keycloak end session endpoint
            var keycloakEndSessionUrl = "https://your-keycloak-server/auth/realms/TestRealm/protocol/openid-connect/logout";
            return Redirect(keycloakEndSessionUrl);
        }
    }
}