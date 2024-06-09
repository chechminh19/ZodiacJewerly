using CloudinaryDotNet.Actions;
using Domain.Entities;
namespace ZodiacJewelryWebApI.Middlewares
{
    public class AdminAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            // Trích xuất token và xác minh (mã bị lược bỏ để ngắn gọn)
            if (!context.User.IsInRole("Admin"))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.WriteAsync("You not allow to do it. !");
                return;
            }

            await _next(context);
        }
    }
}
