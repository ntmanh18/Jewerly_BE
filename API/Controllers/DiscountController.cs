using Azure.Core;
using Bussiness.Services.DiscountService;
using Bussiness.Services.UserService;
using Data.Model.DiscountModel;
using Data.Model.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }
        [Route("view-discount")]
        [HttpGet]
        public async Task<IActionResult> ViewDiscount([FromQuery] DiscountQueryModel query)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _discountService.GetAllDiscount(token, query);
            return StatusCode(res.Code, res);
        }
    

        [Route("create-discount")]
        [HttpPost]
        public async Task<IActionResult> CreateDiscount(CreateDiscountReqModel req)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _discountService.CreateDiscount(token, req);
            return StatusCode(res.Code, res);
        }


        [Route("create-discount-product")]
        [HttpPost]
        public async Task<IActionResult> CreateDiscountProduct(CreateDiscountProductReqModel req) { 
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _discountService.CreateDiscountProduct(token, req);
            return StatusCode(res.Code, res);
        }
        [Route("update-discount")]
        [HttpPut]
        public async Task<IActionResult> UpdateDiscount(UpdateDiscountReqModel req)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _discountService.UpdateDiscount(token, req);
            return StatusCode(res.Code, res);
        }
        [Route("delete-discount")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDiscount([FromQuery] string discountid)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _discountService.DeleteDiscount(token,discountid);
            return StatusCode(res.Code, res);
        }
    }
}
