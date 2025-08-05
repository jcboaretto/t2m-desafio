using Gerenciamento_de_Tarefas.Application.DTOs;
using Gerenciamento_de_Tarefas.Application.Interfaces;
using Gerenciamento_de_Tarefas.Domain.Entities;
using Gerenciamento_de_Tarefas.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gerenciamento_de_Tarefas.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _usuarioRepository;

        public TokenService(IConfiguration configuration, IUsuarioRepository usuarioRepository)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<string> GenerateToken(LoginDTO loginDTO)
        {
            var userDataBase = await _usuarioRepository.BuscarPorUserNameAsync(loginDTO.UserName);

            if (userDataBase == null)
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, userDataBase.Password))
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: new[]
                {
            new Claim(ClaimTypes.NameIdentifier, userDataBase.Id.ToString()),
            new Claim(ClaimTypes.Name, userDataBase.UserName),
            new Claim("id", userDataBase.Id.ToString()),
            new Claim("username", userDataBase.UserName),
            new Claim("nome", userDataBase.Nome)
                },
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

    }
}

