using Bussiness.Services.AccountService;
using Data.Model.AuthenticateModel;
using Data.Model.ResultModel;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : Controller
    {
        private readonly IAccountService _accountService;
        public AuthorizeController(IAccountService accountService)
        {
            _accountService = accountService;   
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginReqModel user)
        {
                ResultModel resultModel = await _accountService.LoginService(user);
            return resultModel.IsSuccess ? Ok(resultModel) : BadRequest(resultModel);
        }
    }
}
