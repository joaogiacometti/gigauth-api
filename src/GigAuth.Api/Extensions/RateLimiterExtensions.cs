using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace GigAuth.Api.Extensions;

public static class RateLimiterExtensions
{
    public static void ConfigureRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            rateLimiterOptions.AddSlidingWindowLimiter("Global", options =>
            {
                options.PermitLimit = 100;
                options.Window = TimeSpan.FromMinutes(1);
                options.SegmentsPerWindow = 10;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 10;
            });
            
            rateLimiterOptions.AddSlidingWindowLimiter("Authorized", options =>
            {
                options.PermitLimit = 1000;
                options.Window = TimeSpan.FromMinutes(1);
                options.SegmentsPerWindow = 10;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 50;
            });
        });
    }
}