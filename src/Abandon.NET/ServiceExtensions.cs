namespace Abandon.NET;

public static class ServiceExtensions 
{
    public static void AddBizService(this IServiceCollection services, IConfiguration configuration)
    {
        // Register HttpClientFactory for API calls
        services.AddHttpClient();
    }
}