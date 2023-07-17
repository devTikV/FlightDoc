namespace FlightDoc.Model
{
    public class BlacklistToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
