using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FlightDoc.Model
{
    public class UserRole : IdentityUserRole<string>
    {
      
        public DateTime CreatedAt { get; set; }
      
    
        // Thêm các trường thông tin khác liên quan đến quyền hạn và vai trò của người dùng
    }
}