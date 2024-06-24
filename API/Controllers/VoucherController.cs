using Azure.Core;
using Bussiness.Services.VoucherService;
using Data.Entities;
using Data.Model.GemModel;
using Data.Model.VoucherModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VoucherController :ControllerBase
    {
        private readonly IVoucherService _voucherService;
        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        [HttpPost("createVoucher")]
        public async Task<ActionResult> CreateVoucher(VoucherCreateModel voucher)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _voucherService.CreateVoucher(token, voucher);
            return StatusCode(res.Code, res);
        }
        [HttpPut("UpdatedVoucher")]
        public async Task<ActionResult> UpdateVoucher(VoucherRequestModel voucherRequest)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _voucherService.UpdateVoucher(token, voucherRequest);
            return StatusCode(res.Code, res);
        }
        [HttpDelete("deleteVoucher")]
        public async Task<ActionResult> DeleteVoucher(string VoucherId)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _voucherService.DeleteVoucherAsync(token, VoucherId);
            return StatusCode(res.Code, res);
        }
        [HttpGet("ViewListVoucher")]
        public async Task<ActionResult> ViewListVoucher([FromQuery] VoucherSearchModel voucherSearch)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _voucherService.ViewListVoucher(token, voucherSearch);
            return StatusCode(res.Code, res);
        }
    }
}
