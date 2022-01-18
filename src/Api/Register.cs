using Amazon.S3;
using Amazon.SQS;
using Infrastructure.S3Service;
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
        
        //AWS services
        services.AddAWSService<IAmazonS3>();
        services.AddAWSService<IAmazonSQS>();

        // Infrastructure
        services.AddSingleton<IS3Service, S3Service>();
        services.AddSingleton<ISqsService, SqsService>();
        
        services.AddWorkers(configuration);
        return services;
    }
}