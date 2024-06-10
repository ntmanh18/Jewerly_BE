using Bussiness.Services.CashierService;
using Bussiness.Services.OldProductService;
using Data.Entities;
using Data.Model.OldProductModel;
using Data.Model.ResultModel;
using Data.Model.OldProductModel;
using Data.Model.ResultModel;
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
        [HttpGet("Get-All")]
        public async Task<ActionResult<IEnumerable<OldProductRequestModel>>> GetAll()
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            //var res = await _cashierService.CreateCashier(token, cashierModel);
            //return StatusCode(res.Code, res);
            var res = await _OPService.GetAllAsync(token);
            return StatusCode(res.Code, res);
        }


        [HttpGet("get-by-Productid")]
        public async Task<ActionResult<IEnumerable<OldProductRequestModel>>> GetByProductId(string productId)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _OPService.GetByProductIdAsync(token, productId);
            return StatusCode(res.Code, res);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ResultModel>> Create([FromBody] OldProductCreateModel oldProduct)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _OPService.AddAsync(token, oldProduct);
            return StatusCode(res.Code, res);
        }
    }
}
