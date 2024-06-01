using Bussiness.Services.UserService;
using Data.Model.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        [HttpPut]

        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserReqModel userModel)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _userService.UpdateUser(token,userModel);
            return StatusCode(res.Code, res);
        }

        [HttpPut]
        [Route("active-deactive-user")]
        public async Task<IActionResult> DeactiveUser([FromQuery(Name = "id")] string userId)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _userService.DeactiveUser(token,new DeactiveUserReqModel { UserId = userId });
            return StatusCode(res.Code, res);
        }

        [HttpPut]
        [Route("update-role")]
        public async Task<IActionResult> UpdateROle([FromBody] UpdateRoleReqModel model)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _userService.UpdateRole(token,model);
            return StatusCode(res.Code, res);
        }

        [HttpGet]
        [Route("view-list-users")]

        public async Task<IActionResult> ViewUserList([FromQuery] UserQueryObject query)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res  = await _userService.ViewUserList(token,query);
            return StatusCode(res.Code, res);
        }

        [HttpPut]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _userService.ChangePassword(token,model);
            return StatusCode(res.Code, res);
        }
    }
}
