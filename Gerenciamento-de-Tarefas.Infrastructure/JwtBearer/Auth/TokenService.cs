//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.IdentityModel.Tokens;
//using Gerenciamento_de_Tarefas.Domain.Entities;
//using System.Text;

//namespace Gerenciamento_de_Tarefas.Infrastructure.JwtBearer.Auth
//{
//    public class TokenService
//    {
//        public string Generate(Usuario usuario)
//        {
//            var handler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes("minha-chave-jwt-super-secreta-1234-gerenciamento-tarefas");

//            var credentials = new SigningCredentials(
//                new SymmetricSecurityKey(key),
//                SecurityAlgorithms.HmacSha256Signature);

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                SigningCredentials = credentials,
//                Expires = DateTime.UtcNow.AddHours(8),
//                // Removendo Claims por enquanto para testar
//            };

//            var token = handler.CreateToken(tokenDescriptor);
//            return handler.WriteToken(token);
//        }
//    }
//}