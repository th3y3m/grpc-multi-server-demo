using ApiGateway.Business.Category;
using ApiGateway.CacheRedis.Cache;
using ApiGateway.ProxyService.Category;
using GrpcContracts.Protos;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpcClient<CategoryService.CategoryServiceClient>(o =>
            {
                o.Address = new Uri(builder.Configuration["AppSettings:CategoryUrl"]);
            });

            var redisHost = builder.Configuration["RedisConfig:Host"];
            var redisPort = builder.Configuration["RedisConfig:Port"];
            var redisPassword = builder.Configuration["RedisConfig:Password"];
            var redisSsl = builder.Configuration["RedisConfig:Ssl"];

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{redisHost}:{redisPort},password={redisPassword},ssl={redisSsl},abortConnect=false";
            });

            builder.Services.AddScoped<ICacheRedis, CacheRedis.Cache.CacheRedis>();

            builder.Services.AddScoped<ICategoryServiceProxy, CategoryServiceProxy>();
            builder.Services.AddScoped<ICategoryBusiness, CategoryBusiness>();

            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy("fixed", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromSeconds(10),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 2
                        }));
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Use rate limiting globally
            app.UseRateLimiter();

            app.MapControllers();

            app.Run();
        }
    }
}
