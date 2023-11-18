using DotNet_Keycloak.business.Model;

namespace DotNet_Keycloak.business.Contract
{
    public interface IUserService
    {
       //Task<string> GetAccessToken();
      public List<UserClaims> GetUserClaims();
    }
}
