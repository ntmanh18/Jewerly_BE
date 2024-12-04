using Bussiness.Services.BillService;
using Bussiness.Services.ProductBillService;
using Data.Entities;
using Data.Model.BillModel;
using Data.Model.ProductBillModel;
using Data.Model.VoucherModel;
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
        [HttpGet("ViewListBill")]
        public async Task<ActionResult> ViewListBill([FromQuery] BillSearchModel billSearch)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _BillService.ViewBill(token, billSearch);
            return StatusCode(res.Code, res);
        }
        [HttpGet("BillCount")]
        public async Task<ActionResult> TotalBill()
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _BillService.BillCount(token);
            return StatusCode(res.Code, res);
        }

        [HttpGet("get-bill-by-cash")]
        public async Task<ActionResult> GetBillByCash()
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _BillService.GetBillByCash(token);
            return StatusCode(res.Code, res);
        }
        [HttpGet("ViewBillById")]
        public async Task<ActionResult> ViewBill([FromQuery] string id)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _BillService.getBillById(token, id);
            return StatusCode(res.Code, res);
        }

        [HttpGet("filter-bill")]
        public async Task<ActionResult> FilterBill([FromQuery]  FilterBillModel filter)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _BillService.FilterBill(token, filter);
            return StatusCode(res.Code, res);
        }
    }
   
}

