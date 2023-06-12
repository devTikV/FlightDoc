/*using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDoc.Model
{
    public class JwtToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TokenId { get; set; }


        public string? Token { get; set; } = null;

       
        public DateTime ExpiresAt { get; set; }

       
        public int UserId { get; set; } 
        public virtual User User { get; set; }
    }
}*/