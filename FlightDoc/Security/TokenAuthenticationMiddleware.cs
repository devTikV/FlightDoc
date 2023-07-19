using FlightDoc.Service;

namespace FlightDoc.Security
{
    // Middleware xác thực token
    public class TokenAuthenticationMiddleware : IMiddleware
    {
        private readonly IBlacklistService _blacklistService;

        public TokenAuthenticationMiddleware(IBlacklistService blacklistService)
        {
            _blacklistService = blacklistService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Lấy access token từ tiêu đề Authorization
            string accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Kiểm tra xem token có nằm trong blacklist hay không
            bool isBlacklisted = await _blacklistService.IsTokenBlacklistedAsync(accessToken);

            if (isBlacklisted)
            {
                Console.WriteLine("Chuỗi tồn tại trong danh sách.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access token is revoked or expired.");
            }
            else
            {
                Console.WriteLine("Chuỗi không tồn tại trong danh sách.");
                await next(context);
            }
        }
    }

}
