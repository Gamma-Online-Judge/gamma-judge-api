using Infrastructure.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<User>> GetAllUsers()
    {
        return Ok();
    }
}
