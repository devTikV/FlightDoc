using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        // Thêm các trường thông tin khác về vai trò

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}