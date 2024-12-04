using Bussiness.Services.DiscountProductService;
using Bussiness.Services.DiscountService;
using Data.Model.DiscountModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountProductController : ControllerBase
    {
        private readonly IDiscountProductService _discountProductService;
        public DiscountProductController(IDiscountProductService discountProductService)
        {
            _discountProductService = discountProductService;   
        }
        [Route("create-discountProduct")]
        [HttpPost]
        public async Task<IActionResult> CreateDiscountProduct( CreateDiscountProductReqModel req)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _discountProductService.CreateDiscountProduct(token, req);
            return StatusCode(res.Code, res);
        }

        [Route("delete-discountProduct")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDiscountProduct(CreateDiscountProductReqModel req)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _discountProductService.DeleteDiscountProduct(token, req);
            return StatusCode(res.Code, res);
        }

    }
}
