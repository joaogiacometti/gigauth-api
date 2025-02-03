using System.Threading.RateLimiting;

namespace GigAuth.Api.Extensions;

public static class RateLimiterExtensions
{
    public static void ConfigureRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy("Global", httpContent =>
                RateLimitPartition.GetSlidingWindowLimiter(
                    httpContent.Connection.RemoteIpAddress?.ToString(),
                    _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 10,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 200
                    }));

            options.AddPolicy("Authorized", httpContent =>
                RateLimitPartition.GetSlidingWindowLimiter(
                    httpContent.User.Identity?.Name?.ToString(),
                    _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 1000,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 100,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 2000
                    }));
        });
    }
}