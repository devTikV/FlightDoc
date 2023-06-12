using BCrypt.Net;
using FlightDoc;
using FlightDoc.Model;
using FlightDoc.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Emit;
using System.Text;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình cơ sở dữ liệu
//db
builder.Services.AddDbContext<FlightDocDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình xác thực JWT
/*var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSecret"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });*/

// Cấu hình CORS (nếu cần thiết)
/*builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});*/

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
// Cấu hình Identity
builder.Services.AddIdentity<ApplicationUser, Role>(options =>
{
    // Cấu hình options cho Identity
    // options.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("Identity:User:RequireUniqueEmail");

    options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:User:Password:RequiredLength");
    options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:User:Password:RequireDigit");
    options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:User:Password:RequireLowercase");
    options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:User:Password:RequireUppercase");
    options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:User:Password:RequireNonAlphanumeric");

   /* options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
    options.SignIn.RequireConfirmedPhoneNumber = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");*/
})
.AddEntityFrameworkStores<FlightDocDb>()
.AddDefaultTokenProviders();

// mã hóa PBDF2
builder.Services.Configure<PasswordHasherOptions>(options =>
{
    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
});

// Đăng ký các dịch vụ và controller
builder.Services.AddControllers();

var app = builder.Build();

// Sử dụng xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();
// middleware 

// Sử dụng routing và controllers
app.MapControllers();

app.Run();
