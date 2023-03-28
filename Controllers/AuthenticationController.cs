using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceReportAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Configuration;
using System.Security.Claims;
using ServiceReportAPI.Contracts;
using System.Net.Mail;
using System.Net;
using ServiceReportAPI.Utils;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceReportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _repository;

        public AuthenticationController(IConfiguration configuration, IUsersRepository repository)
        {
            _configuration = configuration; 
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/<AuthenticationController>
        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            if (user is null)
            {
                return BadRequest("Invalid user request!!!");
            }

            var result = _repository.GetUser(user).Result;

            if (result == null)
                return Unauthorized();

            if (result is User)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Utils.ConfigurationManager.AppSetting["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(issuer: Utils.ConfigurationManager.AppSetting["JWT:ValidIssuer"], audience: Utils.ConfigurationManager.AppSetting["JWT:ValidAudience"], claims: new List<Claim>(), expires: DateTime.Now.AddMinutes(6), signingCredentials: signinCredentials);
                var tokenString = CreateToken();
                var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                result.RefreshToken = refreshToken;
                result.RefreshTokenExpiryTime = tokenString.ValidTo;

                var updated =  _repository.UpdateUser(result);

                Console.WriteLine(updated);

                return Ok(new JWTTokenResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(tokenString),
                    RefreshToken = refreshToken,
                    Expiration = tokenString.ValidTo,
                    User = result
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            var user = await _repository.GetUser(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = CreateToken();
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _repository.UpdateUser(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [HttpGet("recoveryPassword/{mail}")]
        public async Task<IActionResult> SendRecoveryMail(string mail)
        {
            if (mail is null)
            {
                return BadRequest("Invalid user request!!!");
            }

            var result = _repository.GetUsers().Result.ToList().Where(x => x.Email == mail).First();

            #region Leer archivo HTML con el formato del correo
            string body = "";
            string[] lines = Mails.GetHTMLContent("Utils/reset-password.html");

            Random random = new Random();
            string codigo = $"{random.Next(101, 999)}{random.Next(101, 999)}";

            foreach (var item in lines)
            {
                if (item.Contains("######"))
                {
                    body += item.Replace("######", codigo);
                }
                else
                {
                    body += item + " ";
                }
            }
            #endregion

            try
            {
                //send mail
                var res = await Mails.SendMail(mail, "Recuperación de contraseña", body);

                return Ok(res);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private JwtSecurityToken CreateToken()
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }

    class JWTTokenResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public User? User { get; set; }
        public DateTime Expiration { get; set; }
    }
}
