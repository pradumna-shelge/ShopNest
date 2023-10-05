using BackEnd.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackEnd.Services
{
    public class Token
    {
        private readonly IConfiguration _configure;

        public Token( )
        {
            _configure = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
        }

        public  string createToken(string user,string role)
        {
            var sec = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configure.GetSection("Jwt:key").Value));
            var cre = new SigningCredentials(sec, SecurityAlgorithms.HmacSha256);
            var clames = new[] {

                  new Claim(ClaimTypes.Name, user),
                new Claim(ClaimTypes.Role,role)


            };
            var tok = new JwtSecurityToken(
                _configure.GetSection("Jwt:Issuer").Value,
                _configure.GetSection("Jwt:Audience").Value,
                clames,
                expires: DateTime.Now.AddMinutes(12),
                signingCredentials: cre
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tok);

            return token;
        }
    }
}
