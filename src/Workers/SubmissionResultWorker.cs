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
    protected override Task<SqsMessageReturn> ProcessMessage(SubmissionResultModel result, CancellationToken cancellationToken)
    {
        var submission = _submissionService.Get(result.Id);
        if (result.Result.Contains("Accepted"))
        {
            submission.Status = SubmissionStatus.Accepted;
        }
        else if (result.Result.Contains("Time Limit Exceeded"))
        {
            submission.Status = SubmissionStatus.TimeLimitExceeded;
        }
        else if (result.Result.Contains("Runtime Error"))
        {
            submission.Status = SubmissionStatus.RuntimeError;
        }
        else if (result.Result.Contains("Wrong Answer"))
        {
            submission.Status = SubmissionStatus.WrongAnswer;
        }

        _submissionService.Update(result.Id, submission);
        return Task.FromResult(SqsMessageReturn.Success);
    }
}