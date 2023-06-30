using Flight_Doc_Manager_Systems.Services;
using FlightDoc.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlightDoc.RestController.Permission_for_Role
{
    [Route("api/v1/permission")]
    [ApiController]
    public class AddRemovePermission : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly FlightDocDb _flightDocDb;
        private readonly IConfiguration _configuration;
        // private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IAuthService _authService;

        public AddRemovePermission(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager,
            SignInManager<ApplicationUser> signInManager, FlightDocDb flightDocDb, IAuthService authService)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _flightDocDb = flightDocDb;
            //   _jwtTokenGenerator = jwtTokenGenerator;
            _authService = authService;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddPermissionToRole(string roleName, string permission)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound();
            }

            await _roleManager.AddClaimAsync(role, new Claim("Permission", $"Permission.{permission}"));

            return Ok();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemovePermissionFromRole(string roleName, string permission)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound();
            }

            await _roleManager.RemoveClaimAsync(role, new Claim("Permission", $"Permission.{permission}"));

            return Ok();
        }

    }
}
