using Infrastructure.SqsService;
using Workers;

namespace Api;

public static class Register
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        // Infrastructure
        services.AddSingleton<ISqsService, SqsService>();
        
        services.AddWorkers(configuration);
        return services;
    }
}