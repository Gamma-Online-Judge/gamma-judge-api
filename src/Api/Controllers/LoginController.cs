using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Api.Models;

namespace BooksApi.Controllers;

[Route("api/login")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly UserService _userService;
    private readonly TokenService _tokenService;

    public LoginController( UserService userService, TokenService tokenService )
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost]
    public IActionResult Authenticate([FromBody] LoginRequest usuarioRequest)
    {
        var usuario = _userService.Get(usuarioRequest.Username, usuarioRequest.Password);

        if (usuario == null)
            return NotFound(new { message = "Usuário ou senha inválidos", statusCode = 404 });

        var token = _tokenService.GenerateToken(usuario);

        usuario.Password = "";

        return Ok(new {
            user = new UserResponse(usuario),
            token = token
        });
    }
}
