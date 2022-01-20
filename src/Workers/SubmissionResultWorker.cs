using Amazon.SQS;
using Microsoft.Extensions.Logging;
using Workers.Models;
using Infrastructure.Entities;

namespace Workers;

public class SubmissionResultWorker : BaseSqsWorker<string>
{
    public SubmissionResultWorker(ILogger<BaseSqsWorker<string>> logger, IAmazonSQS amazonSqs) : base(logger, amazonSqs)
    {
    }

    protected override string QueueUrl => Contraints.SubmissionResultQueueUrl;
    protected override async Task<SqsMessageReturn> ProcessMessage(string message, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        return SqsMessageReturn.Success;
    }
}