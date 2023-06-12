/*using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDoc.Model
{
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }

      
        [MaxLength(50)]
        public string Name { get; set; }

        // Thêm các trường thông tin khác về quyền hạn

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}*/