/*using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class UserPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Khóa ngoại cho User
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        // Khóa ngoại cho Permission
        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}*/