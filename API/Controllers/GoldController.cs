using Bussiness.Services.CustomerService;
using Bussiness.Services.GoldService;
using Data.Model.CustomerModel;
using Data.Model.GoldModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GoldController : ControllerBase
    {
        private readonly IGoldService _goldService;
        public GoldController(IGoldService goldService)
        {
            _goldService = goldService;
        }

        [HttpGet("get-golds")]
        public async Task<ActionResult> GetGolds()
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _goldService.GetGolds(token);
            return StatusCode(res.Code, res);
        }

        [HttpGet("get-gold-by-id")]
        public async Task<ActionResult> GetGoldsById([FromQuery] string? id)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _goldService.GetGoldById(token, id);
            return StatusCode(res.Code, res);

        }

        [HttpPost("create-gold")]
        public async Task<ActionResult> CreateGold([FromBody] GoldCreateModel goldModel)
        {
            string userIdClaim = null;
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            foreach (var claim in claims)
            {
                if (claim.Type == "userid")
                {
                    userIdClaim = claim.Value;
                }
            }
            var userId = userIdClaim;
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _goldService.CreateGold(token, goldModel, userId);
            return StatusCode(res.Code, res);
        }

        [HttpPut("gold-Update")]
        public async Task<ActionResult> UpdateProduct(GoldUpdateModel goldUpdate)
        {
            string userIdClaim = null;
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            foreach (var claim in claims)
            {
                if (claim.Type == "userid")
                {
                    userIdClaim = claim.Value;
                }
            }
            var userId = userIdClaim;
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _goldService.UpdateGold(token, goldUpdate, userId);
            return StatusCode(res.Code, res);
        }

        [HttpDelete("delete-golds")]
        public async Task<ActionResult> DeleteGolds()
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _goldService.DeleteListGold(token);
            return StatusCode(res.Code, res);
        }
    }
}
