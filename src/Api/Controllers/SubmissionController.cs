using Amazon.SQS.Model;
using Api.Models;
using Infrastructure.S3Service;
using Infrastructure.SqsService;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("")]
public class SubmissionController : ControllerBase
{
    private readonly ILogger<SubmissionController> _logger;
    private readonly ISqsService _sqsService;
    private readonly IS3Service _s3Service;

    public SubmissionController(ILogger<SubmissionController> logger, ISqsService sqsService, IS3Service s3Service)
    {
        _logger = logger;
        _sqsService = sqsService;
        _s3Service = s3Service;
    }

    [HttpPost]
    [Route("judge")]

    public async Task<IActionResult> PostSubmission(ICollection<IFormFile> files, CancellationToken cancellationToken)
    {
        if (files is null || files.Count == 0)
            return BadRequest(
                "Did not receive a file to process."
            );
        var file = files.First();
        var objectId = await _s3Service.SubmitFile(file.FileName, file.OpenReadStream(), cancellationToken);
        return Ok(objectId);
    }

    [HttpGet]
    [Route("submission/{fileKey}")]

    public async Task<IActionResult> GetSubmission([FromRoute] string fileKey, CancellationToken cancellationToken)
    {
        var stream = await _s3Service.GetSubmissionFile(fileKey, cancellationToken);
        return File(stream, "text/plain");
    }
}
