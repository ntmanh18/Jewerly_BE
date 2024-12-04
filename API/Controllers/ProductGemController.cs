using Bussiness.Services.ProductGemService;
using Data.Model.ProductGemModel;
using Data.Model.ProductModel;
using Data.Model.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductGemController : Controller
    {
        private readonly IProductGemService _productGemService;
        public ProductGemController(IProductGemService productGemService)
        {
            _productGemService = productGemService;
        }
        [Route("create-productgem")]
        [HttpPost]
        public async Task<IActionResult> CreateProductGem([FromBody] ProductGemReqModel req)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _productGemService.CreateProductGem(token, req);
            return StatusCode(res.Code, res);
        }

        [Route("delete-productgem")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProductGem(DelteProductGemReqModel req)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _productGemService.DeleteProductGem(token, req);
            return StatusCode(res.Code, res);
        }

        [Route("update-productgem")]
        [HttpPut]
        public async Task<IActionResult> UpdateProductGem(ProductGemReqModel req)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _productGemService.UpdateProductGem(token, req);
            return StatusCode(res.Code, res);
        }
    }
}
