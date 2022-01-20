using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("")]
public class SubmissionController : ControllerBase
{
    private readonly ILogger<SubmissionController> _logger;
    private readonly SqsService _sqsService;
    private readonly S3Service _s3Service;

    public SubmissionController(ILogger<SubmissionController> logger, SqsService sqsService, S3Service s3Service)
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
