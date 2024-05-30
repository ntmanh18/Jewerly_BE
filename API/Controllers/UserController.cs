using Bussiness.Services.UserService;
using Data.Model.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Route("create-user")]
        [HttpPost]
        
        public async Task<IActionResult> CreateUser([FromBody]CreateUserReqModel userModel)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _userService.CreateUser(token,userModel);
            return StatusCode(res.Code, res);
        }

        [Route("update-user")]
        [HttpPost]

        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserReqModel userModel)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _userService.UpdateUser(token,userModel);
            return StatusCode(res.Code, res);
        }
    }
}
