using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Workers;

public static class Register
{
    public static void AddWorkers(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetAWSOptions();
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddHostedService<SubmissionResultWorker>();
    }
    
    private static TType GetConfiguration<TType>(
        this IConfiguration configuration) where TType : class, new()
    {
        var typeConfig = new TType();
        configuration.Bind(typeConfig.GetType().Name, typeConfig);
        return typeConfig;
    }
}