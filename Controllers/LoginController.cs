using CollegeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration; 
        }
        [HttpPost]
        public ActionResult Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid) { return BadRequest("Provide username and pass"); }

            LoginResponseDTO response = new LoginResponseDTO()
            { username = model.username };
            if (model.username == "aum" && model.password == "123")
            {
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtSecret"));
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        //username
                        new Claim(ClaimTypes.Name, model.username),
                        //Role
                        new Claim(ClaimTypes.Role, "Admin")
                    }),
                    Expires = DateTime.Now.AddHours(4),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.token = tokenHandler.WriteToken(token);
                return Ok(response.token); 
            }
            else
            {
                return BadRequest("Invalid username/password");
            }
        }
    }

}
