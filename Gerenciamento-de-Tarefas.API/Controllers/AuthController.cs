using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_de_Tarefas.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : Controller
    {
        private readonly ITokenService _tokenService;
        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var token = await _tokenService.GenerateToken(loginDTO);

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Usuário ou senha inválidos");

            return Ok(token);
        }

    }
}
