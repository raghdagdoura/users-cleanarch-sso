using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UsersApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
[HttpPost("login")]
public IActionResult Login([FromQuery] string email)
{
var key = Encoding.UTF8.GetBytes("SUPER_SECRET_KEY_123456789_SUPER_SECRET_KEY_123456789");

var role = email == "admin@test.com" ? "Admin" : "Manager";

var claims = new[]
{
new Claim(ClaimTypes.Email, email),
new Claim(ClaimTypes.Role, role)
};

var token = new JwtSecurityToken(
claims: claims,
expires: DateTime.UtcNow.AddHours(1),
signingCredentials: new SigningCredentials(
new SymmetricSecurityKey(key),
SecurityAlgorithms.HmacSha256)
);

var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

return Ok(new
{
token = tokenString,
role = role
});
}
}