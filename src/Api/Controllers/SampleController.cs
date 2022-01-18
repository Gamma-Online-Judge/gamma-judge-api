using Amazon.SQS.Model;
using Api.Models;
using Infrastructure.S3Service;
using Infrastructure.SqsService;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("")]
public class SampleController : ControllerBase
{
    private readonly ILogger<SampleController> _logger;
    private readonly ISqsService _sqsService;
    private readonly IS3Service _s3Service;

    public SampleController(ILogger<SampleController> logger, ISqsService sqsService, IS3Service s3Service)
    {
        _logger = logger;
        _sqsService = sqsService;
        _s3Service = s3Service;
    }

    [HttpGet]
    [Route("")]

    public async Task<IActionResult> SamplePost(CancellationToken cancellationToken)
    {
        return Ok(await _s3Service.ListObjects("", cancellationToken));
    }
}
