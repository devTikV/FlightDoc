namespace FlightDoc.Dto
{
    public class AccountDto
    {
        public class RegisterDto
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string FullName { get; set; }
            public string Password { get; set; }
        }

        public class LoginDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class RefreshTokenDto
        {
            public string RefreshToken { get; set; }
        }

        public class AuthenticationResult
        {
            public bool IsSuccess { get; set; }
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public IEnumerable<string> Errors { get; set; }
        }

    }
}
