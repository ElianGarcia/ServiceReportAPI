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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceReportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUsersRepository _repository;

        public AuthenticationController(IUsersRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/<AuthenticationController>
        [HttpPost("login")]
        public IActionResult Login(User user)
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
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new JWTTokenResponse
                {
                    Token = tokenString,
                    User = result
                });
            }
            return Unauthorized();
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
    }

    class JWTTokenResponse
    {
        public string? Token { get; set; }
        public User? User { get; set; }
    }
}
