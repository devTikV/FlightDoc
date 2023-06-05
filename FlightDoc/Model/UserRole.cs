using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [ForeignKey("Flight")]
        public int? FlightId { get; set; }
        public Flight Flight { get; set; }

        // Thêm các trường thông tin khác liên quan đến quyền hạn và vai trò của người dùng
    }
}