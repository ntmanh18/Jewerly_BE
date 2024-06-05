using Bussiness.Services.GemService;
using Microsoft.AspNetCore.Authorization;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Data.Model.GemModel;

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
        [HttpGet]
        public async Task<ActionResult> GetGem()
        {
            var result = await _gemService.GetGem();
            return Ok(result.ToList());
        }
        [HttpGet("gemByName")]
        public async Task<ActionResult> GetGemsByName([FromQuery] string? searchGemName)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _gemService.GetGemByName(token, searchGemName);
            return StatusCode(res.Code, res);
        }
        [HttpGet("gemById")]
        public async Task<ActionResult> GetGemsById([FromQuery] string? searchGemId)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _gemService.GetGemById(token, searchGemId);
            return StatusCode(res.Code, res);
        }
        [HttpPut("createGem")]
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
