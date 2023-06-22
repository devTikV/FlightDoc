using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace FlightDoc.Model
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

       
        public string Email { get; set; }

       
        public string Password { get; set; } 

        [MaxLength(12)]
        public string? CCCD { get; set; }

        [MaxLength(20)]
        public string? Passport { get; set; }

        public bool active { get; set; } = false;
        public string RefreshToken { get; internal set; }
        public DateTime RefreshTokenExpiryTime { get; internal set; }
        // Thêm các trường thông tin khác về người dùng


        /*public virtual ICollection<JwtToken>? JwtTokens { get; set; }*/
    }
}
