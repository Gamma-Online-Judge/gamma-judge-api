using Amazon.SQS.Model;

namespace Infrastructure.SqsService;

public interface ISqsService
{
    public Task<SendMessageResponse> EnqueueAsync<T>(T message, CancellationToken cancellationToken);
}