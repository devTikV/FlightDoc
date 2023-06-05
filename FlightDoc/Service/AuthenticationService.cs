/*using FlightDoc.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using static FlightDoc.Dto.AccountDto;

namespace FlightDoc.Service
{
    public interface IAuthenticationService
    {
        Task<Dto.AccountDto.AuthenticationResult> RegisterAsync(RegisterDto model);
        Task<Dto.AccountDto.AuthenticationResult> LoginAsync(LoginDto model);
        Task<Dto.AccountDto.AuthenticationResult> RefreshTokenAsync(string refreshToken);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthenticationService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<AuthenticationResult> RegisterAsync(RegisterDto model)
        {
            // Kiểm tra logic và tạo người dùng mới
            var user = new User { UserName = model.Username, Email = model.Email, FullName = model.FullName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return new AuthenticationResult { IsSuccess = true };
            }

            return new AuthenticationResult { IsSuccess = false, Errors = result.Errors };
        }

        public async Task<AuthenticationResult> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return new AuthenticationResult { IsSuccess = false, Errors = new[] { "Invalid username or password." } };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return new AuthenticationResult { IsSuccess = false, Errors = new[] { "Invalid username or password." } };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokenResult = await _tokenService.GenerateTokenAsync(user, roles);

            return new AuthenticationResult { IsSuccess = true, Token = tokenResult.Token, RefreshToken = tokenResult.RefreshToken };
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string refreshToken)
        {
            var tokenResult = await _tokenService.RefreshTokenAsync(refreshToken);

            if (tokenResult == null)
            {
                return new AuthenticationResult { IsSuccess = false, Errors = new[] { "Invalid refresh token." } };
            }

            return new AuthenticationResult { IsSuccess = true, Token = tokenResult.Token, RefreshToken = tokenResult.RefreshToken };
        }
    }

}
*/