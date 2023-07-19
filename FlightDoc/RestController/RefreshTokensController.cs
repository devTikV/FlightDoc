using Flight_Doc_Manager_Systems.Models;
using Flight_Doc_Manager_Systems.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FlightDoc.Model;
using Microsoft.AspNetCore.Authentication;
using FlightDoc.Service;
using System.IdentityModel.Tokens.Jwt;

namespace Flight_Doc_Manager_Systems.RestControllers
{
    [Route("api/v1/account/logout")]
    [ApiController]
    public class RefreshTokensController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IBlacklistService _blacklistService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public RefreshTokensController(IAuthService authService, IConfiguration configuration, UserManager<ApplicationUser> userManager, IBlacklistService blacklistService)
        {
            _authService = authService;
            _blacklistService = blacklistService;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(GetRefreshTokenViewModel model)
        {
            try
            {
                if (model is null)
                {
                    return BadRequest("Invalid client request");
                }

                var result = await _authService.GetRefreshToken(model);
                if (result.StatusCode == 0)
                    return BadRequest(result.StatusMessage);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            // Lấy access token từ tiêu đề Authorization
            string accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Giải mã access token và lấy thông tin expiration time
            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(accessToken);
            var expirationTime = decodedToken.ValidTo;

            // Thêm access token vào danh sách đen
            await _blacklistService.AddToBlacklistAsync(accessToken, expirationTime);

            // Đăng xuất người dùng
            await HttpContext.SignOutAsync();

            return Ok("Logged out successfully.");
        }


        [Authorize]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }
            return Ok("Success");
        }
    }
}
