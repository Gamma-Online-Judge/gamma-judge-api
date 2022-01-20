using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/submissions")]
public class SubmissionController : ControllerBase
{
    private readonly SubmissionService _submissionService;

    public SubmissionController(SubmissionService submissionService)
    {
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
        [FromForm] SubmissionRequest submissionRequest,
        CancellationToken cancellationToken)
    {
        if (submissionRequest.File is null)
            return BadRequest("Did not receive a file to process.");
        try{
            var submission = submissionRequest.ToSubmission();
            return Ok(await _submissionService.Create(submission, submissionRequest.File.OpenReadStream(), cancellationToken));
        }
        catch(ArgumentNullException e){
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("status/{id}")]

    public IActionResult GetSubmissionStatus([FromRoute] string id)
    {
        return Ok(_submissionService.Get(id));
    }

    [HttpGet]
    [Route("file/{id}")]

    public async Task<IActionResult> GetSubmissionFile([FromRoute] string id, CancellationToken cancellationToken)
    {
        var stream = await _submissionService.GetFile(id, cancellationToken);
        return File(stream, "text/plain");
    }
}
