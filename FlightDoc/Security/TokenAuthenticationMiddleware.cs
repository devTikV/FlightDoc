using FlightDoc.Service;

namespace FlightDoc.Security
{
    // Middleware xác thực token
    public class TokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBlacklistService _blacklistService;

        public TokenAuthenticationMiddleware(RequestDelegate next, IBlacklistService blacklistService)
        {
            _next = next;
            _blacklistService = blacklistService;
        }

        public async Task Invoke(HttpContext context)
        {
            // Lấy access token từ tiêu đề Authorization
            string accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Kiểm tra xem token có nằm trong blacklist hay không
            var stringList = await _blacklistService.GetBlacklistAsync();

            bool exists = stringList.Contains(accessToken);
            Console.WriteLine("aaa" + String.Join(", ", stringList));

            if (exists)
            {
                Console.WriteLine("Chuỗi tồn tại trong danh sách.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access token is revoked or expired.");
                return;
            }
            else
            {
                Console.WriteLine("Chuỗi không tồn tại trong danh sách.");
                await _next(context);
            }
        }
    }
}
