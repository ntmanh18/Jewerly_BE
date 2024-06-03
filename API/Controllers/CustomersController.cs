using Bussiness.Services.CustomerService;
using Data.Model.CustomerModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RTools_NTS.Util;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        

        [HttpPost]
        public async Task<ActionResult> CreateCustomer([FromBody] CustomerCreateModel customerModel)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _customerService.CreateCustomer(token, customerModel);
            return StatusCode(res.Code, res);
            //var result = await _customerService.CreateCustomer(customerModel);
            //return Ok(result);
        }
    }
}
