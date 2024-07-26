using Bussiness.Services.DashBoardService;
using Bussiness.Services.DiscountService;
using Data.Model.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashBoardController : Controller
    {
        private readonly IDashService _dashService;
        public DashBoardController(IDashService dashService)
        {
            _dashService = dashService;
        }
        [HttpGet("TotalIncome")]
        public async Task<ActionResult<ResultModel>> GetTotalIncome()
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _dashService.GetTotalIncomeAsync(token);
            return StatusCode(result.Code, result);
        }

        [HttpGet("IncomeByCashNumber")]
        public async Task<ActionResult<ResultModel>> GetIncomeByCashNumber()
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _dashService.GetIncomeByCashNumberAsync(token);
            return StatusCode(result.Code, result);
        }

        [HttpGet("IncomeByMonth")]
        public async Task<ActionResult<ResultModel>> GetIncomeByMonth()
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _dashService.GetIncomeByMonthAsync(token);
            return StatusCode(result.Code, result);
        }
    }
}
