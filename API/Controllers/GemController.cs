using Bussiness.Services.GemService;
using Microsoft.AspNetCore.Authorization;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Data.Model.GemModel;
using Data.Model.VoucherModel;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GemController : ControllerBase
    {
        private readonly IGemService _gemService;
        public GemController(IGemService gemService)
        {
            _gemService = gemService;
        }
        [HttpGet("ViewListGem")]
        public async Task<ActionResult> ViewListVoucher([FromQuery] GemSearchModel gemSearch)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _gemService.ViewListGem(token, gemSearch);
            return StatusCode(res.Code, res);
        }
        [HttpPost("createGem")]
        public async Task<ActionResult> CreateGem(CreateGemModel creategem)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _gemService.CreateGem(token, creategem);
            return StatusCode(res.Code, res);
        }
        [HttpPut("updateGem")]
        public async Task<ActionResult> updateGem(GemRequestModel creategem)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _gemService.UpdateGem(token, creategem);
            return StatusCode(res.Code, res);
        }
    }
}
