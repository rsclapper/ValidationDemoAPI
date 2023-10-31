using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ValidationDemoApi.AuthService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest user)
    {
        if (user == null)
        {
            return BadRequest("Invalid request");
        }
        if (user.Username != "admin" || user.Password != "admin")
        {
            return Unauthorized();
        }
        // generate a JWT token
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyForSignInSecret@1234"));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "https://localhost:7124",
            audience: "https://localhost:7124",
            claims: new List<Claim>() { new Claim("UserId", "1")},
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: signinCredentials);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
           
        return Ok(new { Token = tokenString});
    }
    
    
}
public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}