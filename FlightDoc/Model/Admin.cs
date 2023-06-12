 using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDoc.Model
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }

       
        [MaxLength(50)]
        public string Name { get; set; }

        // Thêm các thuộc tính thông tin khác về quản trị viên

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}