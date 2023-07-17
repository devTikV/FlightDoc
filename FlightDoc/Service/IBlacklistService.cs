namespace FlightDoc.Service
{
    public interface IBlacklistService
    {
        Task AddToBlacklistAsync(string token);
        public Task<List<string>> GetBlacklistAsync();
    }
}
