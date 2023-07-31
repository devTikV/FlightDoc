namespace FlightDoc.Dto
{
    public class UserWithRolesDto
    {
        public AccountDto User { get; set; }
        public List<string> Roles { get; set; }
    }

}
