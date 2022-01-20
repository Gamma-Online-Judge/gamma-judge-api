using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Infrastructure.Entities;

namespace Infrastructure.Services;

public class SqsService
{

    private readonly IAmazonSQS _sqsClient;
    public SqsService(IAmazonSQS sqsClient)
    {
        _sqsClient = sqsClient;
    }

    public async Task<SendMessageResponse> EnqueueSubmissionc(Submission submission, CancellationToken cancellationToken)
    {
        string jsonString = JsonSerializer.Serialize(submission);
        return await _sqsClient.SendMessageAsync(Contraints.SubmissionsQueueUrl, jsonString, cancellationToken);
    }
}