using Data.Model.Payment;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.PaymentService
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, PaymentRequestModel model);

        PaymentResponseModel MakePayment(IQueryCollection colletions);
    }
}
