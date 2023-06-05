using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        // Thêm các trường thông tin khác về quyền hạn

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}