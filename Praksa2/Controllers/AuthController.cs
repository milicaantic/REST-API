using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Praksa2.Models;
using Praksa2.Dtos;
using Praksa2.Services;
using Praksa2.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using BusinessLogicLayer.Dtos;
using DataAccessLayer.Models;
using BusinessLogicLayer.Services;


namespace Praksa2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthController(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            if (await _userRepository.GetUserByUsernameAsync(registerDto.Username) != null)
            {
                return BadRequest("Username already exists.");
            }

            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = PasswordService.HashPassword(registerDto.Password)
            };

            await _userRepository.AddUserAsync(user);

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);

            if (user == null || !PasswordService.VerifyPassword(user.PasswordHash, loginDto.Password))
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, user.Username), new("UserID", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            return Ok(new { Token = tokenString });
        }

        /*[HttpPost("logout")]
        public IActionResult Logout()
        {
           
            return Ok("Logged out successfully.");
        }*/
    }
}
