using Amazon.SQS;
using Microsoft.Extensions.Logging;
using Workers.Models;
using Infrastructure.Entities;
using Infrastructure.Services;

namespace Workers;

public class SubmissionResultModel
{
    public string Id { get; set; } = "";
    public string Result { get; set; } = "";
}
public class SubmissionResultWorker : BaseSqsWorker<SubmissionResultModel>
{
    private readonly SubmissionService _submissionService;
    private readonly ILogger<BaseSqsWorker<SubmissionResultModel>> _logger;
    public SubmissionResultWorker(
        ILogger<BaseSqsWorker<SubmissionResultModel>> logger,
        IAmazonSQS amazonSqs,
        SubmissionService submissionService) : base(logger, amazonSqs)
    {
        _submissionService = submissionService;
        _logger = logger;
    }
    protected override string QueueUrl => Contraints.SubmissionResultQueueUrl;
    protected override int DelayAfterNoMessage => 2;
    protected override Task<SqsMessageReturn> ProcessMessage(SubmissionResultModel result, CancellationToken cancellationToken)
    {
        var submission = _submissionService.Get(result.Id);
        submission.SubmissionDetails = result.Result;
        submission.Status = result.Result.ToSubmissionStatus();

        _submissionService.Update(result.Id, submission);
        return Task.FromResult(SqsMessageReturn.Success);
    }
}