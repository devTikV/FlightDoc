using FlightDoc.Dto;
using FlightDoc.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FlightDoc.RestController
{
    [ApiController]
    [Route("api/v1/account")]
    public class GetUserAndRole : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserAndRole(RoleManager<Role> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("role")]
        public async Task<ActionResult<List<Role>>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("user")]
        public async Task<List<AccountDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDTOs = users.Select(user => new AccountDto(user)).ToList();
            return userDTOs;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{user}")]
        public async Task<ActionResult<List<string>>> GetUserRolesByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles.ToList());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("permissionRole/{role}")]
        public async Task<ActionResult<List<string>>> GetRolePermissionsAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound();
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var rolePermissions = roleClaims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();

            return Ok(rolePermissions);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("permission/{user}")]
        public async Task<ActionResult<List<string>>> GetUserPermissionsAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var userPermissions = new List<string>();

            foreach (var roleName in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    var rolePermissions = roleClaims.Where(c => c.Type == "Permission").Select(c => c.Value);
                    userPermissions.AddRange(rolePermissions);
                }
            }

            return Ok(userPermissions);
        }
    }
}
