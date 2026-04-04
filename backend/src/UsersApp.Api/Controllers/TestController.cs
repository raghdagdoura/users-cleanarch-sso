using Microsoft.AspNetCore.Mvc;
using UsersApp.Application.Common.Interfaces;

namespace UsersApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IEmailSender _emailSender;

    public TestController(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    [HttpGet("send-email")]
    public async Task<IActionResult> SendEmail()
    {
        try {
        await _emailSender.SendAsync(
            "raghda.gdoura1@gmail.com",
            "Test SendGrid 🚀",
            "<h1>Ça marche ! 🎉</h1>",
            CancellationToken.None
        );

        return Ok("Email envoyé !");
        }
        
            catch (Exception ex)
{
    return BadRequest(ex.Message);
}
        
    }
}