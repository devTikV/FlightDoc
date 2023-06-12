/*using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDoc.Model
{
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        // Quan hệ Token - User (Một token được liên kết với một người dùng)
        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}*/