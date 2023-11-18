using DotNet_Keycloak.business.Contract;
using DotNet_Keycloak.business.Model;
using Microsoft.AspNetCore.Http;

namespace DotNet_Keycloak.business.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
   
        public List<UserClaims> GetUserClaims()
        {
            return _httpContextAccessor.HttpContext.User.Claims
                .Select(claim => new UserClaims
                {
                    Type = claim.Type,
                    Value = claim.Value
                })
                .ToList();
        }
    }
}
