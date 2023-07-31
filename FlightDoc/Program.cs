using BCrypt.Net;
using FlightDoc.Model;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.StaticFiles;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

using Flight_Doc_Manager_Systems.Services;
using FlightDoc.Service;
using ApplicationUser = FlightDoc.Model.ApplicationUser;
using FlightDoc.Security;
using FlightDoc.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

//db
builder.Services.AddDbContext<FlightDocDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<TokenAuthenticationMiddleware>();
builder.Services.AddScoped<IBlacklistService, BlacklistService>();

//Đọc cấu hình từ appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

//cấu hình permission
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadFilePolicy", policy =>
    {
        policy.RequireAuthenticatedUser(); // Yêu cầu người dùng xác thực
        policy.RequireClaim("Permission", "Permission.ReadFile"); // Yêu cầu claim "Permission.ReadFile"
    });

    options.AddPolicy("UpFilePolicy", policy =>
    {
        policy.RequireAuthenticatedUser(); // Yêu cầu người dùng xác thực
        policy.RequireClaim("Permission", "Permission.UpFile"); // Yêu cầu claim "Permission.UpFile"
    });

    options.AddPolicy("DownloadFilePolicy", policy =>
    {
        policy.RequireAuthenticatedUser(); // Yêu cầu người dùng xác thực
        policy.RequireClaim("Permission", "Permission.DownloadFile"); // Yêu cầu claim "Permission.DownloadFile"
    });

    options.AddPolicy("DeleteFilePolicy", policy =>
    {
        policy.RequireAuthenticatedUser(); // Yêu cầu người dùng xác thực
        policy.RequireClaim("Permission", "Permission.DeleteFile"); // Yêu cầu claim "Permission.DeleteFile"
    });
});

// Cấu hình dịch vụ tĩnh
builder.Services.AddScoped<ImanageImage, ManageDocFlightSystem>();
builder.Services.AddTransient < ImanageImage, ManageDocFlightSystem> ();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<Role>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
// mã hóa PBDF2
builder.Services.Configure<PasswordHasherOptions>(options =>
{
    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
});

// Cấu hình Google Authentication


// Cấu hình Identity
builder.Services.AddIdentity<ApplicationUser, Role>
    (options =>
    {
        // Cấu hình options cho Identity
        options.Password.RequiredLength = configuration.GetValue<int>("Identity:User:Password:RequiredLength");
        options.Password.RequireDigit = configuration.GetValue<bool>("Identity:User:Password:RequireDigit");
        options.Password.RequireLowercase = configuration.GetValue<bool>("Identity:User:Password:RequireLowercase");
        options.Password.RequireUppercase = configuration.GetValue<bool>("Identity:User:Password:RequireUppercase");
        options.Password.RequireNonAlphanumeric = configuration.GetValue<bool>("Identity:User:Password:RequireNonAlphanumeric");
    })
.AddEntityFrameworkStores<FlightDocDb>()
.AddDefaultTokenProviders();

// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWTKey:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]))
                };
            });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseMiddleware<TokenAuthenticationMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// CORS
app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("https://localhost:3000", "http://localhost:5000", "https://localhost:5001")
                .AllowCredentials());

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
