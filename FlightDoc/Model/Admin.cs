using System.ComponentModel.DataAnnotations;

namespace FlightDoc.Model
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        // Thêm các thuộc tính thông tin khác về quản trị viên

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}