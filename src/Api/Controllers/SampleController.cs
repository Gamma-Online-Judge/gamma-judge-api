using Amazon.SQS.Model;
using Api.Models;
using Infrastructure.SqsService;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("")]
public class SampleController : ControllerBase
{
    private readonly ILogger<SampleController> _logger;
    private readonly ISqsService _sqsService;

    public SampleController(ILogger<SampleController> logger, ISqsService sqsService)
    {
        _logger = logger;
        _sqsService = sqsService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [Route("post")]

    public async Task<SendMessageResponse> SamplePost([FromBodyAttribute] SampleRequest request, CancellationToken cancellationToken)
    {
        
        return await _sqsService.EnqueueAsync(request, cancellationToken);
    }
}
