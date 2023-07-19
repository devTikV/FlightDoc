using Flight_Doc_Manager_Systems.Services;
using FlightDoc.Dto;
using FlightDoc.Helper;
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
        public async Task<IActionResult> AddPermissionToRole([FromBody] PermissionDto permissionDto)
        {
            var role = await _roleManager.FindByNameAsync(permissionDto.RoleName);
            if (role == null)
            {
                return NotFound();
            }

            var existingClaim = (await _roleManager.GetClaimsAsync(role))
                .FirstOrDefault(c => c.Type == "Permission" && c.Value == $"Permission.{permissionDto.Permission}");

            if (existingClaim != null)
            {
                return BadRequest("Quyền đã tồn tại trong vai trò.");
            }

            await _roleManager.AddClaimAsync(role, new Claim("Permission", $"Permission.{permissionDto.Permission}"));

            return Ok($"Thêm quyền {permissionDto.Permission} vào vai trò {permissionDto.RoleName} thành công.");
        }



        [HttpPost("remove")]
        public async Task<IActionResult> RemovePermissionFromRole([FromBody] PermissionDto permissionDto)
        {
            var role = await _roleManager.FindByNameAsync(permissionDto.RoleName);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var existingClaim = await _roleManager.GetClaimsAsync(role);
            var claimToRemove = existingClaim.FirstOrDefault(c => c.Type == "Permission" && c.Value == $"Permission.{permissionDto.Permission}");
            if (claimToRemove != null)
            {
                await _roleManager.RemoveClaimAsync(role, claimToRemove);
                return Ok("Permission removed from role successfully.");
            }
            else
            {
                return BadRequest("Permission not found in role.");
            }
        }
    }
}