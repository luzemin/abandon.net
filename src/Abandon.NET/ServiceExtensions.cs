using Abandon.NET.Utility.DataBase;
using Abandon.NET.Utility.Logger;
using Abandon.NET.Utility.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Abandon.NET;

public static class ServiceExtensions 
{
    public static void AddBizService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<INLogHelper, NLogHelper>();
        services.AddSingleton<ISqlSugarFactory, SqlSugarFactory>();
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        
        // 注册Redis
        var redisConnectionString = configuration["Redis:ConnectionString"];
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(redisConnectionString);
            });
            services.AddSingleton<IRedisHelper, RedisHelper>();
        }
    }
}