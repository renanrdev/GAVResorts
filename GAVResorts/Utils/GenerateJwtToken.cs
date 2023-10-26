using GAVResorts.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GAVResorts.Utils
{
    public class GenerateJwtToken
    {
        public static string GenerateToken(User user, IConfiguration configuration)
        {
            var claims = new List<Claim>
            {
                new Claim("Nome", user.Usuario),
                new Claim("Role", user.TipoUsuario)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: expiry,
                signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
