using FlightDoc;
using FlightDoc.Dto;
using FlightDoc.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

[Route("api/v1/account")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly FlightDocDb flightDocDb;
    public UserController(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AccountDto request)
    {
        if (ModelState.IsValid)
        {
            if (request.Email.EndsWith("@vietjetair.com"))
            {
                var user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.Email,
                    FullName = request.FullName,
                    CCCD = request.CCCD,
                    Passport = request.Passport,
                    Password = request.Password
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                }
            }
            else
            {
                return BadRequest("vui lòng dk email có đuôi \"@vietjetair.com ");
            }

            return Ok("thêm thành công user");
        }
        // Trả về lỗi nếu dữ liệu không hợp lệ
        return BadRequest(ModelState);
    }

    [HttpPost("users/{userId}/roles/{roleName}")]
    public async Task<IActionResult> AddUserToRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var role = await _roleManager.FindByNameAsync(roleName);
        if (user != null)
        {
           await _userManager.AddToRoleAsync(user, roleName);

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
                CreatedAt = DateTime.UtcNow
            };
            await flightDocDb.UserRoles.AddAsync(userRole);
            await flightDocDb.SaveChangesAsync();

            return Ok("User added to role successfully.");
        }

        return NotFound("User not found.");
    }



    // login
    [HttpPost("dangnhap")]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AccountDto model)
    {
        // Chuẩn hóa email
        var normalizedEmail = model.Email.Trim().ToLower();

        // Tìm kiếm người dùng trong cơ sở dữ liệu
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);

        if (user == null)
        {
            // Người dùng không tồn tại
            return StatusCode(401, "Email không hợp lệ.");
        }

        var result = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!result)
        {
            // Mật khẩu không đúng
            return StatusCode(401, "Mật khẩu không đúng.");
        }

        return Ok("Login thành công!");
    }
}
/*[HttpGet("layvaitro_user")]
public async Task<IActionResult> GetIdUser([FromBody] AccountDto model)
{
    string userId = "Id của người dùng";

    var user = await _userManager.FindByIdAsync(userId);

    if (user != null)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        // userRoles chứa danh sách các vai trò của người dùng
        foreach (var role in userRoles)
        {
            // Do something with the role
        }
    }
    return Ok("lấy thành công  " + UserRoles);
}*/
