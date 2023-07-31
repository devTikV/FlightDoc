using FlightDoc.Model;

namespace FlightDoc.Dto
{
    public class AccountDto
    {
        public string FullName { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CCCD { get; set; }
        public string Passport { get; set; }
        public string? Role { get; set; } 

        public AccountDto( ) { } // Cần thêm constructor mặc định cho việc deserialization

        public AccountDto(ApplicationUser user)
        {
            UserName = user.UserName;
            Email = user.Email;
            FullName = user.FullName;
            CCCD = user.CCCD;
            Passport = user.Passport;
        }
    }
}
