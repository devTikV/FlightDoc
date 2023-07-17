using Microsoft.Extensions.Caching.Memory;
using FlightDoc.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightDoc.Service
{  
        public class BlacklistService : IBlacklistService
        {
            private readonly IMemoryCache _cache;
            private readonly FlightDocDb _dbContext;
          
        public BlacklistService(IMemoryCache cache, FlightDocDb dbContext )
            {
                _cache = cache;
                _dbContext = dbContext;
             
            }

            public async Task AddToBlacklistAsync(string token, DateTime expirationDate)
            {
                var blacklistToken = new BlacklistToken
                {
                    Token = token,
                    ExpirationDate = expirationDate
                };

                _dbContext.BlacklistTokens.Add(blacklistToken);
                await _dbContext.SaveChangesAsync();

                CleanupExpiredTokens();
                await CacheBlacklistTokens();

                await Task.CompletedTask;
            }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            var blacklist = await _cache.GetOrCreateAsync("blacklist", async entry =>
            {
                // Kiểm tra xem cache còn tồn tại hay không
                if (entry == null)
                {
                    // Thực hiện truy vấn cơ sở dữ liệu để lấy danh sách token
                    var tokens = await _dbContext.BlacklistTokens.Select(t => t.Token).ToListAsync();
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(30)); // Cache for 30 minutes
                    entry.Value = tokens; // Cập nhật danh sách token trong cache
                }

                return entry.Value as List<string>;
            });

            return blacklist.Contains(token);
        }




        private void CleanupExpiredTokens()
            {
                var expiredTokens = _dbContext.BlacklistTokens
                    .Where(t => t.ExpirationDate < DateTime.UtcNow)
                    .ToList();

                _dbContext.BlacklistTokens.RemoveRange(expiredTokens);
                _dbContext.SaveChanges();
            }

            private async Task CacheBlacklistTokens()
            {
                var tokens = await _dbContext.BlacklistTokens.Select(t => t.Token).ToListAsync();
                _cache.Set("blacklist", tokens, TimeSpan.FromMinutes(30)); // Cache for 30 minutes
            }
        }
    }


