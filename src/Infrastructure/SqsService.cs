using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Infrastructure.SqsService;

public class SqsService
{

    private readonly IAmazonSQS _sqsClient;
    private readonly string _queueUrl = "https://sqs.sa-east-1.amazonaws.com/818598312538/CompileJobs";

    public SqsService(IAmazonSQS sqsClient)
    {
        _sqsClient = sqsClient;
    }

    public async Task<SendMessageResponse> EnqueueAsync<T>(T message, CancellationToken cancellationToken)
    {
        var messageRequest = new SendMessageRequest
        {
            QueueUrl = _queueUrl,
            MessageBody = JsonSerializer.Serialize(message)
        };
        return await _sqsClient.SendMessageAsync(messageRequest, cancellationToken);
    }
}