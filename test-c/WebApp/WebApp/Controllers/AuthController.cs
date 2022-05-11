using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using WebApp.Model;

namespace WebApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        this._config = config;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([FromBody] Login userLogin)
    {
        var user = Authenticate(userLogin);
        if (user != null)
        {
            var token = Generate(user);
            return Ok(token);
        }

        return NotFound("User not found");
    }

    private string Generate(User user) // Генератор токена
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials    = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Login),
            // new Claim();
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private User Authenticate(Login userLogin)
    {
        var user = UsersCurrent.db.FirstOrDefault(user =>
            user.Login.ToLower() == userLogin.UserLogin && user.Password == userLogin.Password);

        if (user != null)
        {
            return user;
        }

        return user;
    }
}