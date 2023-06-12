/*using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class UserToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Khóa ngoại cho User
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        // Khóa ngoại cho Token
        [ForeignKey("Token")]
        public int TokenId { get; set; }
        public virtual Token Token { get; set; }
    }
}*/