
using Flight_Doc_Manager_Systems.Models;
using Flight_Doc_Manager_Systems.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FlightDoc.Model;

namespace Flight_Doc_Manager_Systems.RestControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokensController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public RefreshTokensController(IAuthService authService, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
          
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
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return Ok("Success");
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