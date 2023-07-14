using Flight_Doc_Manager_Systems.Models;
using Flight_Doc_Manager_Systems.Services;
using FlightDoc;
using FlightDoc.Dto;
using FlightDoc.Model;
using FlightDoc.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using ApplicationUser = FlightDoc.Model.ApplicationUser;

[Route("api/v1/account")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly FlightDocDb _flightDocDb;
    private readonly IConfiguration _configuration;
   // private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IAuthService _authService;

    public UserController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager,
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
                    UserName = request.FullName,
                    FullName = request.FullName,
                    CCCD = request.CCCD,
                    Passport = request.Passport,
                    Password = request.Password
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    // Đăng nhập người dùng sau khi đăng ký thành công (tuỳ chọn)
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return Ok("Đăng ký thành công");
                }
                else
                {
                    // Xử lý lỗi khi ghi nhận người dùng
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                
            }
            return BadRequest("vui lòng dk email có đuôi \"@vietjetair.com ");
        }
        // Trả về lỗi nếu dữ liệu không hợp lệ
        return BadRequest(ModelState);
    }

    [HttpPost("admin/register")]
    public async Task<IActionResult> RegisterAdmin([FromBody] AccountDto request)
    {
        if (ModelState.IsValid)
        {
            if (request.Email.EndsWith("@vietjetair.com"))
            {
                var user = new ApplicationUser
                {
                   
                    UserName = request.Email,
                    NormalizedEmail = request.Email,
                    Email = request.Email,
                    CCCD = request.CCCD,
                    Passport = request.Passport,
                    FullName = request.FullName,
                    Password = request.Password
                };
                var result = await _userManager.CreateAsync(user, request.Password);

                // Tạo vai trò "admin" nếu chưa tồn tại
                var adminRole = new Role { Name = "Admin" };
                await _roleManager.CreateAsync(adminRole);

                await _userManager.AddToRoleAsync(user, "Admin");
                var role = await _roleManager.FindByNameAsync("Admin");

                return Ok("đăng ký thành công user role admin");

            }

            return BadRequest("đăng ký email với domain @vietjetair.com");

        }
        return BadRequest(ModelState);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("admin/register/roleUser/{UserName}")]
    public async Task<IActionResult> RegisterRoleUser(string UserName, [FromBody] RoleDto roleDto)
    {
        var user = await _userManager.FindByEmailAsync(UserName);
        if (user == null)
        {
            return BadRequest("Sai tên user!");
        }

        // Kiểm tra xem giá trị roleName có khác null hoặc rỗng không
        if (string.IsNullOrEmpty(roleDto.Name))
        {
            return BadRequest("Tên vai trò không được để trống!");
        }

        // Kiểm tra xem vai trò đã tồn tại hay chưa
        var existingRole = await _roleManager.FindByNameAsync(roleDto.Name);
        if (existingRole == null)
        {
            // Tạo vai trò nếu chưa tồn tại
            var createRole = new Role { Name = roleDto.Name };
            var createResult = await _roleManager.CreateAsync(createRole);
            if (!createResult.Succeeded)
            {
                return BadRequest("Tạo vai trò thất bại!");
            }
        }

        // Thêm vai trò cho người dùng
        var addToRoleResult = await _userManager.AddToRoleAsync(user, roleDto.Name);
        if (!addToRoleResult.Succeeded)
        {
            return BadRequest("Đăng ký vai trò cho user thất bại!");
        }

        // Lấy thông tin của vai trò đã tạo
        var role = await _roleManager.FindByNameAsync(roleDto.Name);
        if (role != null)
        {
            return Ok($"Đăng ký thành công user role admin {role.Name}");
        }

        return BadRequest("Đăng ký vai trò cho user thất bại!");
    }


    [HttpPost("login")]
    
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");
            var result = await _authService.Login(model);
            if (result.StatusCode == 0)
                return BadRequest(result.StatusMessage);
            return Ok(result);
        }
        catch (Exception ex)
        {
          
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
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
