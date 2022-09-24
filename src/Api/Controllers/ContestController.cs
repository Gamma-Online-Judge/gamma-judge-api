using Infrastructure.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Api.Models;

namespace BooksApi.Controllers;

[Route("api/contests")]
[ApiController]
public class ContestController : ControllerBase
{
    private readonly ContestService _contestService;
    private readonly ProblemService _problemService;

    public ContestController(
        ContestService contestService,
        ProblemService problemService)
    {
        _contestService = contestService;
        _problemService = problemService;
    }

    [HttpGet]
    public ActionResult<List<Contest>> QueryContests(
        [FromQuery] string? name,
        [FromQuery] int? limit,
        [FromQuery] int? skip
    )
    {
        var contests = _contestService.QueryByName(name ?? "", limit ?? 10, skip ?? 0);
        return Ok(contests.Select(BuildContestResponse).ToList());
    }

    [HttpGet("{id}", Name = "GetContest")]
    public ActionResult<Contest> Get(string id)
    {
        var contest = _contestService.Get(id);

        if (contest == null)
        {
            return NotFound();
        }

        return Ok(BuildContestResponse(contest));
    }

    [HttpPost]
    public ActionResult<Contest> Create(Contest contest)
    {
        _contestService.Create(contest);
        return CreatedAtRoute("GetContest", new { id = contest?.Id?.ToString() }, contest);
    }

    [HttpPut]
    public ActionResult<Contest> CreateOrUpdate(Contest contest)
    {
        _contestService.CreateOrUpdate(contest);
        return CreatedAtRoute("GetContest", new { id = contest?.Id?.ToString() }, contest);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, Contest contestIn)
    {
        var contest = _contestService.Get(id);

        if (contest == null)
        {
            return NotFound();
        }

        _contestService.Update(id, contestIn);

        return Ok(BuildContestResponse(_contestService.Get(id)));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        if (!_contestService.Exists(id))
        {
            return NotFound();
        }
        _contestService.Remove(id);

        return NoContent();
    }

    public ProblemShortResponse BuildProblemShortResponse(Problem problem)
    {
        var contest = problem.ContestId is null ? null : _contestService.Get(problem.ContestId);
        return new ProblemShortResponse(problem, contest);
    }

    public ContestResponse BuildContestResponse(Contest contest)
    {
        var problems = contest.Problems.Select(contestProblem => _problemService.Get(contestProblem.CustomId)).Select(BuildProblemShortResponse).ToList();
        return new ContestResponse(contest, problems);
    }
}
