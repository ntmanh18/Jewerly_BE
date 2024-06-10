using Bussiness.Services.CashierService;
using Bussiness.Services.OldProductService;
using Data.Model.OldProductModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OldProductController : ControllerBase
    {
        private readonly IOldProductService _OPService;
        public OldProductController(IOldProductService OPService)
        {
            _OPService = OPService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OldProductRequestModel>>> GetAll()
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            //var res = await _cashierService.CreateCashier(token, cashierModel);
            //return StatusCode(res.Code, res);
            var res = await _OPService.GetAllAsync(token);
            return StatusCode(res.Code, res);
        }


        [HttpGet("byProduct/{productId}")]
        public async Task<ActionResult<IEnumerable<OldProductRequestModel>>> GetByProductId(string productId)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _OPService.GetByProductIdAsync(token, productId);
            return StatusCode(res.Code, res);
        }
    }
}
