using Infrastructure.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers
{
    [Route("api/contests")]
    [ApiController]
    public class ContestController : ControllerBase
    {
        private readonly ContestService _contestService;

        public ContestController(
            ContestService contestService)
        {
            _contestService = contestService;
        }

        [HttpGet]
        public ActionResult<List<Contest>> Get() => _contestService.Get();

        [HttpGet("{id}", Name = "GetContest")]
        public ActionResult<Contest> Get(string id)
        {
            var contest = _contestService.Get(id);

            if (contest == null)
            {
                return NotFound();
            }

            return contest;
        }

        [HttpPost]
        public ActionResult<Contest> Create(Contest contest)
        {
            _contestService.Create(contest);
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

            return Ok(_contestService.Get(id));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var contest = _contestService.Get(id);
            if (!_contestService.Exists(id) || contest?.Id is null)
            {
                return NotFound();
            }
            _contestService.Remove(contest.Id);

            return NoContent();
        }
    }
}