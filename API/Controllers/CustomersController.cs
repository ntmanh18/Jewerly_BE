using Bussiness.Services.CustomerService;
using Data.Entities;
using Data.Model.CustomerModel;
using Data.Model.ProductModel;
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

        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {

            var result = await _customerService.GetCustomers();
            return Ok(result.ToList());
        }

        [HttpGet("customerName")]
        public async Task<ActionResult> GetCustomersByName([FromQuery] string? searchCustomerName)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _customerService.GetCustomersByName(token, searchCustomerName);
            return StatusCode(res.Code, res);
            
        }

        [HttpGet("phone")]
        public async Task<ActionResult<Product>> GetByPhone(string phone)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _customerService.GetCustomerByPhone(token, phone);
            return StatusCode(res.Code, res);
        }

        [HttpPut("customerIdUpdate")]
        public async Task<ActionResult> UpdateProduct(CustomerUpdateModel customerUpdate)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _customerService.UpdateCustomer(token, customerUpdate);
            return StatusCode(res.Code, res);
        }
    }
}
