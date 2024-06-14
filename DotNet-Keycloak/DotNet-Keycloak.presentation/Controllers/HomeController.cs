using DotNet_Keycloak.business.Contract;
using DotNet_Keycloak.presentation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNet_Keycloak.presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        static readonly HttpClient client = new HttpClient();

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var userClaims = _userService.GetUserClaims()
                .Select(claim => new UserClaimsModel { Type = claim.Type, Value = claim.Value }).ToList();

            userClaims.Add(new UserClaimsModel { Type = "user_id", Value = currentUserId });
            userClaims.Add(new UserClaimsModel { Type = "access_token", Value = accessToken });
            userClaims.Add(new UserClaimsModel { Type = "id_token", Value = idToken });
            userClaims.Add(new UserClaimsModel { Type = "refresh_token", Value = refreshToken });

            return View(userClaims);
        }

        //public async Task<IActionResult> UserRole()
        //{
        //    // Retrieve the access token from HttpContext
        //    var accessToken = await HttpContext.GetTokenAsync("access_token");

        //    // Decode the access token
        //    var handler = new JwtSecurityTokenHandler();
        //    var jsonToken = handler.ReadJwtToken(accessToken);

        //    // Extract 'resource_access' claim
        //    var resourceAccessClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "resource_access")?.Value;

        //    // Prepare a list to hold user claims
        //    var claimsModel = new List<UserClaimsModel>();

        //    // Parse 'resource_access' JSON if available
        //    if (!string.IsNullOrEmpty(resourceAccessClaim))
        //    {
        //        var resourceAccess = JObject.Parse(resourceAccessClaim);

        //        // Example: Extract roles for a specific client (e.g., TestClient)
        //        var testClientRoles = resourceAccess["TestClient"]?["roles"]?.ToObject<string[]>();
        //        if (testClientRoles != null)
        //        {
        //            foreach (var role in testClientRoles)
        //            {
        //                claimsModel.Add(new UserClaimsModel { Type = "user_Role", Value = role });
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // Handle case where 'resource_access' claim is missing or empty
        //        // Example: Add default roles or log an error
        //        claimsModel.Add(new UserClaimsModel { Type = "user_Role", Value = "DefaultRole" });
        //    }

        //    // Return the view with the list of claims
        //    return View(claimsModel);
        //}

        public IActionResult Logout()
        {
            // Clear authentication cookies
             HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
             HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            // Redirect to Keycloak end session endpoint
            var keycloakEndSessionUrl = "http://host.docker.internal:9080/realms/TestRealm/protocol/openid-connect/logout";

            keycloakEndSessionUrl += "?redirect_uri=" + Url.Action("Index", "Home", null, Request.Scheme);

            return Redirect(keycloakEndSessionUrl);
        }


    }
}