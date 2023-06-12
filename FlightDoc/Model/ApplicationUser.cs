using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDoc.Model
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

       
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } 

        [MaxLength(12)]
        public string? CCCD { get; set; } = null;

        [MaxLength(20)]
        public string? Passport { get; set; } = null;

        // Thêm các trường thông tin khác về người dùng

      
        /*public virtual ICollection<JwtToken>? JwtTokens { get; set; }*/
    }
}
