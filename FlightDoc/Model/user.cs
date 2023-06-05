using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class User
    {

        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(12)]
        public string CCCD { get; set; }

        [MaxLength(20)]
        public string Passport { get; set; }

        // Thêm các trường thông tin khác về người dùng

        public ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<JwtToken> JwtTokens { get; set; }
    }
}
