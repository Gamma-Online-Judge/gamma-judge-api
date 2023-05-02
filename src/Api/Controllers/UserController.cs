using Infrastructure.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace BooksApi.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(
        UserService userService
    )
    {   
        _userService = userService;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<List<UserResponse>> GetAllUsers()
    {
        var users = _userService.Get();
        var response = new List<UserResponse>();
        foreach (var user in users)
            response.Add(new UserResponse(user));

        return response;
    }

    [HttpGet("{id}", Name = "GetUser")]
    [Authorize]
    public ActionResult<UserResponse> GetUser(string id)
    {
        return new UserResponse(_userService.Get(id));
    }

    [HttpPost]
    public ActionResult<UserResponse> CreateUser(User user)
    {
        return new UserResponse(_userService.Create(user));
    }
}
