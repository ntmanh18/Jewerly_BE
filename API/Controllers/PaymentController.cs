using Azure.Core;
using Bussiness.Services.BillService;
using Bussiness.Services.PaymentService;
using Data.Model.BillModel;
using Data.Model.Payment;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
        [Route("create-paymenturl")]
        [HttpPost]


        public IActionResult CreatePaymenturl([FromBody] PaymentRequestModel req)
        {
            
            var res = _vnPayService.CreatePaymentUrl(HttpContext, req);
            if (res == null)
            {
                return StatusCode(500, "Create failed");
            }
            else return Ok(res);
        }

        [Route("make-payment")]
        [HttpGet]


        public IActionResult MakePayment()
        {

            var res = _vnPayService.MakePayment(Request.Query);
            if (res == null)
            {
                return StatusCode(500, "Payment failed");
            }
            else return Ok(res);
        }
    }
}
