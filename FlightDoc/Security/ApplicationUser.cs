global using Microsoft.AspNetCore.Identity;

namespace FlightDoc.Security
{


      public class ApplicationUser : IdentityUser
        {
        // Thêm các thuộc tính thông tin khác về người dùng
        
        public string FullName { get; set; } = null!;
            public string FirstName { get; set; }=null!;
            public string LastName { get; set; }=null!;
            

        }

     
    
}
