using Bussiness.Services.UserService;
using Data.Model.UserModel;
using Internship.Business.Services.EmailService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EmailController : Controller
    {

        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
           _emailService = emailService;
        }
        [Route("send-mail")]
        [HttpPost]


        public async Task<IActionResult> SendMail([FromBody] EmailSendingFormat email)
        {
            await _emailService.SendEmail(email);

            return Ok();
        }
    }
}
