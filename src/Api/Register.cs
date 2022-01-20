using Amazon.S3;
using Amazon.SQS;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Workers;

namespace Api;

public static class Register
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers().AddNewtonsoftJson();
        services.AddEndpointsApiExplorer();

        // Database
        services.Configure<JudgeDatabaseSettings>(configuration.GetSection(nameof(JudgeDatabaseSettings)));

        services.AddSingleton<IJudgeDatabaseSettings>(sp =>
        sp.GetRequiredService<IOptions<JudgeDatabaseSettings>>().Value);

        services.AddSingleton<ContestService>();
        services.AddSingleton<ProblemService>();
        services.AddSingleton<SubmissionService>();

        //AWS services
        services.AddAWSService<IAmazonS3>();
        services.AddAWSService<IAmazonSQS>();

        // Infrastructure
        services.AddSingleton<SqsService>();


        services.AddWorkers(configuration);
        return services;
    }
}