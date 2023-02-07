using System.Net.Security;
using Infrastructure.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

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
            usuario = usuario,
            token = token
        });
    }
 
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<dynamic>> Get() => "Autenticado";
}
