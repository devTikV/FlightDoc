/*using FlightDoc.Dto;
using FlightDoc.Model;
using FlightDoc.Repositories;
using FlightDoc.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightDoc.Service
{
    public class AccountService : IAccountRepository
    {
        private readonly FlightDocDb _dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        
        // kiểm tra Accounts có địa chỉ email khớp với email được cung cấp
        //  StringComparison.OrdinalIgnoreCase != hoa thường
        public bool Exists(string email)
        {
            return _dbContext.Users.Any(a => a.Email!=email);
        }
        public AccountService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        public async Task<string> SignInAsync(UserDto model)
        {
            var result = await signInManager.PasswordSignInAsync
                (model.Email, model.Password, false, false);
                if(!result.Succeeded)
            {
                return string.Empty;
            }
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString
                    ())
                };
            var authenKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(configuration
                ["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey,
                SecurityAlgorithms.HmacSha512Signature)
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
        }      

        public async Task<IdentityResult> SignUpAsync(UserDto model)
        {
            var user = new ApplicationUser
            {
                Username = model.Username,
                
                FullName = model.FullName,
                Email = model.Email,
                CCCD = model.CCCD,
                Passport = model.Passport
            };
            
            
            return await userManager.CreateAsync(user, model.Password);
        }

        public Task<string> SignInAsync(User model)
        {
            throw new NotImplementedException();
        }

        *//*public async Task<IdentityResult> SignUpAsync(UserDto model)
        {
            var user = new User
            {
                Username = model.Username,
                Password = model.Password,
                FullName = model.FullName,
                Email = model.Email,
                CCCD = model.CCCD,
                Passport = model.Passport
            };

            // Lưu người dùng vào cơ sở dữ liệu
            _dbContext.Users.Add(user);
            var data = await _dbContext.SaveChangesAsync();
            return await userManager.CreateAsync(user, model.Password);
        }*//*
    }

}
*/