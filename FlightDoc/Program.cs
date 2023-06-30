using BCrypt.Net;
using FlightDoc;
using FlightDoc.Model;
using FlightDoc.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.StaticFiles;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using FlightDoc.Security.Aspnet_Identity.Configuration;
using Flight_Doc_Manager_Systems.Services;
using FlightDoc.Service;

var builder = WebApplication.CreateBuilder(args);

//db
builder.Services.AddDbContext<FlightDocDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
        policy.RequireClaim("Permission", "ReadFile"); // Yêu cầu claim "ReadFile"
    });

    options.AddPolicy("UpFilePolicy", policy =>
    {
        policy.RequireAuthenticatedUser(); // Yêu cầu người dùng xác thực
        policy.RequireClaim("Permission", "UpFile"); // Yêu cầu claim "EditFile"
    });
    options.AddPolicy("DownloadFilePolicy", policy =>
    {
        policy.RequireAuthenticatedUser(); // Yêu cầu người dùng xác thực
        policy.RequireClaim("Permission", "DownloadFile"); // Yêu cầu claim "DownloadFile"
    });
    options.AddPolicy("DeleteFilePolicy", policy =>
    {
        policy.RequireAuthenticatedUser(); // Yêu cầu người dùng xác thực
        policy.RequireClaim("Permission", "DeleteFile"); // Yêu cầu claim "DeleteFile"
    });
});
// Cấu hình dịch vụ tĩnh
builder.Services.AddScoped<ImanageImage, ManageDocFlightSystem>();
builder.Services.AddTransient < ImanageImage, ManageDocFlightSystem> ();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<Role>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
// mã hóa PBDF2
builder.Services.Configure<PasswordHasherOptions>(options =>
{
    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
});
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
                .WithOrigins("http://localhost:3000/")
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
/*
var builder = WebApplication.CreateBuilder(args);

// Cấu hình cơ sở dữ liệu
//db
builder.Services.AddDbContext<FlightDocDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đọc cấu hình từ appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();


builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<Role>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
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

// cookie của identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "MyAppAuthCookie";
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
});



// Cấu hình JWT Bearer Authentication
// Jwt Configuration
var jwtSettings = new JwtSettings();
builder.Configuration.Bind(JwtSettings.SectionName, jwtSettings);

builder.Services.AddSingleton(Options.Create(jwtSettings));

builder.Services.AddScoped<JwtTokenGenerator>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

// mã hóa PBDF2
builder.Services.Configure<PasswordHasherOptions>(options =>
{
    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
});

// Đăng ký các dịch vụ và controller
builder.Services.AddControllers();
var app = builder.Build();
//cor
app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("http://localhost:3000/")
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());
// Sử dụng xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();

// Sử dụng routing và controllers
app.MapControllers();

app.Run();
*/