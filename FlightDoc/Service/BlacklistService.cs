using System.IdentityModel.Tokens.Jwt;

namespace FlightDoc.Service
{
    public class BlacklistService : IBlacklistService
    {
        private readonly List<string> _blacklist;

        public BlacklistService()
        {
            _blacklist = new List<string>();
        }

        public Task AddToBlacklistAsync(string token)
        {
            _blacklist.Add(token);
            Console.WriteLine(String.Join(", ", _blacklist));
            CleanupExpiredTokens();
            return Task.CompletedTask;
        }

        private void CleanupExpiredTokens()
        {
            List<string> tokensToRemove = new List<string>();

            foreach (var token in _blacklist)
            {
                if (IsTokenExpired(token))
                {
                    tokensToRemove.Add(token);
                }
            }

            foreach (var token in tokensToRemove)
            {
                _blacklist.Remove(token);
            }
        }

        private bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var expirationDate = jwtToken.ValidTo;
            var now = DateTime.UtcNow;

            return now > expirationDate;
        }

        public Task<List<string>> GetBlacklistAsync()
        {
            return Task.FromResult(_blacklist);
        }
    }
}
