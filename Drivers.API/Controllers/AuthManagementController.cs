using Drivers.API.Configuration;
using Drivers.API.Models.DtOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Drivers.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthManagementController : ControllerBase
    {
        private readonly ILogger<AuthManagementController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthManagementController(ILogger<AuthManagementController> logger,
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionMonitor)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtConfig = optionMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requesDto)
        {
            if (ModelState.IsValid)
            {
                //Check if Email Exist
                var EmailExist = await _userManager.FindByEmailAsync(requesDto.Email);
                if (EmailExist != null)
                {
                    return BadRequest("Email Aready Exist");
                }
                var newUser = new IdentityUser()
                {
                    Email = requesDto.Email,
                    UserName = requesDto.Email
                };
                var isCreated = await _userManager.CreateAsync(newUser, requesDto.Password);
                if (isCreated.Succeeded)
                {
                    var token = GenerateJwtToken(newUser);
                    return Ok(new RegistrationRequestResponse()
                    {
                        Result = true,
                        Token = token
                    });
                }
                return BadRequest(isCreated.Errors.Select(x => x.Description).ToList());
            }
            return BadRequest("Invalid Request Payload");
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto requesDto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(requesDto.Email);
                if (existingUser == null)
                {
                    return BadRequest("Invalid authentication");
                }
                var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, requesDto.Password);
                if (isPasswordValid)
                {
                    var token = GenerateJwtToken(existingUser);
                    return Ok(new LoginRequestResponse
                    {
                        Token = token,
                        Result = true
                    });
                }
                return BadRequest("Invalid authentication");
            }
            return BadRequest("Invalid Request Payload");
        }


        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
