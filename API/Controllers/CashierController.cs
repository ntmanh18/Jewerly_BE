using Bussiness.Services.CashierService;
using Bussiness.Services.CustomerService;
using Data.Model.CashierModel;
using Data.Model.CustomerModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        public async Task<ActionResult> CreateCashier([FromBody] CashierRequestModel cashierModel)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _cashierService.CreateCashier(token, cashierModel);
            return StatusCode(res.Code, res);
        }
    }
}
