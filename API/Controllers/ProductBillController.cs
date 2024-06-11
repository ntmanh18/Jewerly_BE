using Bussiness.Services.ProductBillService;
using Data.Model.ProductBillModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductBillController : Controller
    {
        private readonly IProductBillService _productBillService;
        public ProductBillController(IProductBillService productBillService)
        {
            _productBillService = productBillService;
        }
        [Route("create-productbill")]
        [HttpPost]


        public async Task<IActionResult> CreateUser([FromBody] CreateProductBillReqModel req)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _productBillService.CreateProductBill(token, req);
            return StatusCode(res.Code, res);
        }
    }
}


