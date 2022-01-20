using Infrastructure.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers
{
    [Route("api/problems")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly ProblemService _problemService;

        public ProblemController(
            ProblemService problemService)
        {
            _problemService = problemService;
        }

        [HttpGet]
        public ActionResult<List<Problem>> Get() => _problemService.Get();

        [HttpGet("{id}", Name = "GetProblem")]
        public ActionResult<Problem> Get(string id)
        {
            var problem = _problemService.Get(id);

            if (problem == null)
            {
                return NotFound();
            }

            return problem;
        }

        [HttpPost]
        public ActionResult<Problem> Create(Problem problem)
        {
            _problemService.Create(problem);
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

            return Ok(_problemService.Get(id));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var problem = _problemService.Get(id);

            if (problem?.Id is null || _problemService.Exists(id))
            {
                return NotFound();

            }
            _problemService.Remove(problem.Id);
            return NoContent();

        }
    }
}