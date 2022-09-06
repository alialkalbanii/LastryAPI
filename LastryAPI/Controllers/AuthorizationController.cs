using LastryAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LastryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;



        public AuthorizationController(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;

        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var x = _context.Accounts.Where(d => d.userName == request.userName).FirstOrDefault();
            if (x == null)
            {
                return BadRequest("User name was not found");
            }

            if (x.passWord != request.password)
            {
                return BadRequest("Wrong Password");
            }
            var token = getToken(x);

            return Ok(token);
        }

        private String getToken(Account user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

                new Claim(ClaimTypes.Name, user.userName),

                new Claim(ClaimTypes.Role, user.role),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}

