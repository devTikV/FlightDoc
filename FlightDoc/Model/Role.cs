using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDoc.Model
{
    public class Role : IdentityRole
    {
        // Các thuộc tính bổ sung cho CustomRole
        public string Description { get; set; }
        // ...
    }
    
}