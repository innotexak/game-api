using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoGameApi.Model;
using VideoGameApi.Services.Interfaces;


namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(IEmailDto payload)
        {
            await _emailService.SendEmail(payload);
            return Ok();
        }

    }
}
