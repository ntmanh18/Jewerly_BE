using Bussiness.Services.CashierService;
using Bussiness.Services.CustomerService;
using Data.Entities;
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

        [HttpGet]
        public async Task<ActionResult> GetCashiers()
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _cashierService.GetAllCashiers(token);
            return StatusCode(res.Code, res);
        }

        //[HttpGet("productId")]
        //public async Task<ActionResult<Cashier>> GetById(string productId)
        //{
        //    var res = await _cashierService.GetCashierById(productId);
        //    return Ok(res);
        //    //var product = await _productService.GetProductById(productId);
        //    //if (product == null)
        //    //{
        //    //    return NotFound("Product not found");
        //    //}

        //    //return Ok(product);
        //}

        [HttpPut("cashierIdUpdate")]
        public async Task<ActionResult> UpdateProduct(CashierUpdateModel cashierUpdate)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _cashierService.UpdateCashier(token, cashierUpdate);
            return StatusCode(res.Code, res);
        }
    }
}
