using Amazon.SQS;
using Microsoft.Extensions.Logging;
using Workers.Models;

namespace Workers;

public class Worker : BaseSqsWorker<string>
{
    public Worker(ILogger<BaseSqsWorker<string>> logger, IAmazonSQS amazonSqs) : base(logger, amazonSqs)
    {
    }

    protected override string QueueUrl => "https://sqs.sa-east-1.amazonaws.com/818598312538/MyQueue";
    protected override async Task<SqsMessageReturn> ProcessMessage(string message, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        return SqsMessageReturn.Success;
    }
}