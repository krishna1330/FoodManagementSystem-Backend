using FMS.Business.Client.Models;
using FMS.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly JwtOptions _options;
        private readonly AuthDAC _authDAC;

        public AuthController(IConfiguration configuration, IOptions<JwtOptions> options, AuthDAC authDAC)
        {
            _configuration = configuration;
            _options = options.Value;
            _authDAC = authDAC;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string emailID, string password)
        {
                    if (string.IsNullOrEmpty(emailID) || string.IsNullOrEmpty(password))
            {
                return BadRequest("EmailID or password cannot be null or empty.");
            }

            try
            {
                AuthorizedUser? user = await _authDAC.IsAuthorizedUser(emailID, password);

                if (user == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error processing the request.");
                }

                switch (user.ResponseMessage)
                {
                    case "Invalid credentials.":
                        return Unauthorized(user.ResponseMessage);

                    case "Your account is inactive or deleted.":
                        return Conflict(user.ResponseMessage);

                    case "Login successful.":
                        user.Token = GetJWTToken(emailID);
                        return Ok(user);

                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected response from authorization service.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        private string GetJWTToken(string email)
        {
            //var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key ?? string.Empty));
            var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var crendential = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Email",email)
            };
            var sToken = new JwtSecurityToken(_options.Key, _options.Issuer, claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: crendential);
            var token = new JwtSecurityTokenHandler().WriteToken(sToken);
            return token;
        }
    }
}
