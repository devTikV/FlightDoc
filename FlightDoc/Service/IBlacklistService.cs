namespace FlightDoc.Service
{
    public interface IBlacklistService
    {
        public Task AddToBlacklistAsync(string token, DateTime expirationDate);
        public Task<bool> IsTokenBlacklistedAsync(string token);

    }
}
