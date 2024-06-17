using Bussiness.Services.BillService;
using Bussiness.Services.ProductBillService;
using Data.Model.BillModel;
using Data.Model.ProductBillModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BillController : Controller
    {
        private readonly IBillService _BillService;
        public BillController(IBillService BillService)
        {
            _BillService = BillService;
        }
        [Route("create-bill")]
        [HttpPost]


        public async Task<IActionResult> CreateBill([FromBody] CreateBillReqModel req)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _BillService.CraeteBill(token, req);
            return StatusCode(res.Code, res);
        }
    }
}
