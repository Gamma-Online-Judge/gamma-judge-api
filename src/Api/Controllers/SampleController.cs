using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("")]
public class SampleController : ControllerBase
{
    private readonly ILogger<SampleController> _logger;

    public SampleController(ILogger<SampleController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [Route("post")]

    public SampleResponse SamplePost([FromBodyAttribute] SampleRequest request)
    {
        return new SampleResponse
        {
            Atribute1 = request.Atribute1,
            Atribute2 = request.Atribute2,
        };
    }
}
