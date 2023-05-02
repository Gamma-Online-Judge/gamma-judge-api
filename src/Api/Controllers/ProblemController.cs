using Api.Models;
using Infrastructure.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers;

[Route("api/problems")]
[ApiController]
public class ProblemController : ControllerBase
{
    private readonly ProblemService _problemService;
    private readonly ContestService _contestService;

    public ProblemController(
        ProblemService problemService,
        ContestService contestService)
    {
        _problemService = problemService;
        _contestService = contestService;
    }

    [HttpGet]
    public ActionResult<List<Problem>> QueryProblems(
        [FromQuery] string? title,
        [FromQuery] int? limit,
        [FromQuery] int? skip
     )
    {
        var problems = _problemService.QueryByTitle(title ?? "", limit ?? 10, skip ?? 0);
        return Ok(problems.Select(BuildProblemResponse).ToList());
    }

    [HttpGet("{id}", Name = "GetProblem")]
    public ActionResult<Problem> Get(string id)
    {
        var problem = _problemService.Get(id);

        if (problem == null)
        {
            return NotFound();
        }

        return Ok(BuildProblemResponse(problem));
    }

    [HttpPost]
    public ActionResult<Problem> Create(Problem problem)
    {
        _problemService.Create(problem);
        return CreatedAtRoute("GetProblem", new { id = problem?.Id?.ToString() }, problem);
    }

    [HttpPut]
    public ActionResult<Problem> CreateOrUpdate(Problem problem)
    {
        _problemService.CreateOrUpdate(problem);
        return CreatedAtRoute("GetProblem", new { id = problem?.Id?.ToString() }, problem);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, Problem problemIn)
    {
        var problem = _problemService.Get(id);

        if (problem == null)
        {
            return NotFound();
        }

        _problemService.Update(id, problemIn);

        return Ok(BuildProblemResponse(_problemService.Get(id)));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        if (!_problemService.Exists(id))
        {
            return NotFound();

        }
        _problemService.Remove(id);

        return NoContent();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ProblemResponse BuildProblemResponse(Problem problem)
    {
        var contest = problem.ContestId is null ? null : _contestService.Get(problem.ContestId);
        return new ProblemResponse(problem, contest);
    }
}
