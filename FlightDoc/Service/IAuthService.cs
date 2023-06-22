using Flight_Doc_Manager_Systems.Models;

namespace Flight_Doc_Manager_Systems.Services
{
    public interface IAuthService
    {
       
        Task<TokenViewModel> Login(LoginModel model);
     
        Task<TokenViewModel> GetRefreshToken(GetRefreshTokenViewModel model);
    }
}
