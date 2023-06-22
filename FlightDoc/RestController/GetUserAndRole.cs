/*using FlightDoc.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlightDoc.RestController
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetUserAndRole : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly FlightDocDb _flightDocDb;
        public GetUserAndRole(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager,
            SignInManager<ApplicationUser> signInManager, FlightDocDb flightDocDb)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _flightDocDb = flightDocDb;
        }
        public async Task<List<IdentityUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<List<string>> GetUserRolesAsync(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}
*/