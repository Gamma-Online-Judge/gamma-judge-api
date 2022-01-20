using Amazon.S3;
using Amazon.SQS;
using contestsApi.Services;
using Infrastructure.S3Service;
using Infrastructure.Settings;
using Infrastructure.SqsService;
using Microsoft.Extensions.Options;
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

        // Database
        services.Configure<JudgeDatabaseSettings>(configuration.GetSection(nameof(JudgeDatabaseSettings)));

        services.AddSingleton<IJudgeDatabaseSettings>(sp =>
        sp.GetRequiredService<IOptions<JudgeDatabaseSettings>>().Value);

        services.AddSingleton<ContestService>();
        services.AddSingleton<ProblemService>();

        //AWS services
        services.AddAWSService<IAmazonS3>();
        services.AddAWSService<IAmazonSQS>();

        // Infrastructure
        services.AddSingleton<S3Service>();
        services.AddSingleton<SqsService>();


        services.AddWorkers(configuration);
        return services;
    }
}