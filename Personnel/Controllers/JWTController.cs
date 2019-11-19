using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Personnel.Helpers;
using Personnel.Models;

namespace Personnel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTController : Controller
    {
        private IConfiguration _config { get; }
        private DatabaseContext _context { get; }

        public JWTController(IConfiguration config, DatabaseContext context)
        {
            _context = context;
            _config = config;
        }
  

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]Login login)
        {
            IActionResult response = Unauthorized();
            var Logins = Authenticate(login);
            if (Logins != null)
            {

                var tokenString = BuildToken(Logins);
                response = Ok(new { token = tokenString,Site=login.Site }) ;
            }
            return response;

        }



        private string BuildToken(Login Logins)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:Expires"]));

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, Logins.Username),
                new Claim(JwtRegisteredClaimNames.Email,Logins.Password),
                new Claim("Myhost","MYHOSTCOMPANY"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private Login Authenticate(Login login)
        {
            var password = EncryptionHelper.Encrypt(login.Password);
            Login users = _context.Logins.FirstOrDefault(
                x => x.Username.Equals(login.Username) && x.Password.Equals(password));
            return users;
        }
    }
}