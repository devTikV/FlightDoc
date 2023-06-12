/*

namespace FlightDoc.Security
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    public class HeaderIdMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Kiểm tra xem header "Id" có tồn tại trong yêu cầu hay không
            if (context.Request.Headers.ContainsKey("Id"))
            {
                // Lấy giá trị của header "Id"
                string idValue = context.Request.Headers["Id"];

                // Lưu giá trị vào HttpContext để có thể sử dụng trong Controller hoặc API endpoints
                context.Items["Id"] = idValue;
            }

            // Chuyển yêu cầu tới middleware tiếp theo
            await _next(context);
        }
    }

}
*/