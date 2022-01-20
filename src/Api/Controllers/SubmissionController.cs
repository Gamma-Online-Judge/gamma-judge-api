using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/submissions")]
public class SubmissionController : ControllerBase
{
    private readonly ILogger<SubmissionController> _logger;
    private readonly SubmissionService _submissionService;

    public SubmissionController(ILogger<SubmissionController> logger, SubmissionService submissionService)
    {
        _logger = logger;
        _submissionService = submissionService;
    }

    [HttpGet]
    [Route("")]

    public IActionResult GetSubmissions()
    {
        return Ok(_submissionService.Get());
    }

    [HttpPost]
    [Route("")]

    public async Task<IActionResult> PostSubmission(
        [FromBody] SubmissionRequest submissionRequest, 
        ICollection<IFormFile> files, 
        CancellationToken cancellationToken)
    {
        if (files is null || files.Count == 0)
            return BadRequest(
                "Did not receive a file to process."
            );
        var file = files.First();
        var submission = submissionRequest.ToSubmission(file.FileName);
        return Ok(await _submissionService.Create(submission, file.OpenReadStream(), cancellationToken));
    }

    [HttpGet]
    [Route("{id}")]

    public IActionResult GetSubmission([FromRoute] string id)
    {
        return Ok(_submissionService.Get(id));
    }
}
