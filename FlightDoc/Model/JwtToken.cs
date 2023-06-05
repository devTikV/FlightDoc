using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class JwtToken
    {
        [Key]
        public int TokenId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}