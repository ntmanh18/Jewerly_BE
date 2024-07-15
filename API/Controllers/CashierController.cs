using Bussiness.Services.CashierService;
using Bussiness.Services.CustomerService;
using Data.Entities;
using Data.Model.CashierModel;
using Data.Model.CustomerModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CashierController : ControllerBase
    {
        private readonly ICashierService _cashierService;
        public CashierController(ICashierService cashierService)
        {
            _cashierService = cashierService;
        }

        [HttpPost("Create-Cashier")]
        public async Task<ActionResult> CreateCashier([FromBody] CashierRequestModel cashierModel)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _cashierService.CreateCashier(token, cashierModel);
            return StatusCode(res.Code, res);
        }

        [HttpGet("Get-Cashiers")]
        public async Task<ActionResult> GetCashiers()
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _cashierService.GetAllCashiers(token);
            return StatusCode(res.Code, res);
        }


        [HttpPut("cashier-Update")]
        public async Task<ActionResult> UpdateProduct(CashierUpdateModel cashierUpdate)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _cashierService.UpdateCashier(token, cashierUpdate);
            return StatusCode(res.Code, res);
        }
        [HttpPut("deactivate")]
        public async Task<ActionResult<Cashier>> DeactiveCashier(string id)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _cashierService.DeactiveCashier(token, id);
            return StatusCode(res.Code, res);
        }

        [HttpGet("Search-By-User-Id")]
        public async Task<ActionResult> GetCashierByUserId([FromQuery] string? id)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _cashierService.GetCashiersByUserId(token, id);
            return StatusCode(res.Code, res);

        }

        [HttpGet("Search-By-Date")]
        public async Task<ActionResult> GetCashierByDate(DateTime date,int num)
        {
            DateTime dateStart;
            DateTime dateEnd;
            try
            {
                dateStart = new DateTime(date.Year, date.Month, date.Day, 00, 00, 01);
                dateEnd = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid date-time parameters: {ex.Message}");
            }

            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _cashierService.GetCashiersByDate(token, dateStart, dateEnd, num);
            return StatusCode(res.Code, res);

        }
        [HttpGet("Income-By-Date")]
        public async Task<ActionResult> GetIncomeByDate(DateTime date, int num)
        {
            
            DateTime dateStart;
            DateTime dateEnd;
            try
            {
                dateStart = new DateTime(date.Year, date.Month, date.Day, 00, 00, 01);
                dateEnd = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid date-time parameters: {ex.Message}");
            }

            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _cashierService.GetIncomeByDate(token, dateStart, dateEnd, num);
            return StatusCode(res.Code, res);

        }
        [HttpPut("Update-Status-Cashier")]
        public async Task<ActionResult<Cashier>> UpdateStatusCashier(string id)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _cashierService.UpdateStatusCashier(token, id);
            return StatusCode(res.Code, res);
        }
    }
}
