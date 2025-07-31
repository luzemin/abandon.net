using SLN.DbHelper;
using SLN.LogHelper;

namespace SLN;

public static class ServiceExtensions 
{
    public static void AddBizService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<INLogHelper, NLogHelper>();
        services.AddSingleton<ISqlSugarFactory, SqlSugarFactory>();
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
    }
}