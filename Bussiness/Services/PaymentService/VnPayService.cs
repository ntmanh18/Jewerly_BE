﻿using Data.Model.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.PaymentService
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;

        public VnPayService(IConfiguration config)
        {
            _config = config;
        }
        public string CreatePaymentUrl(HttpContext context, PaymentRequestModel model)
        {
            var now = DateTime.Now;
            var tick = now.Ticks;
            var expiredDate = now.AddMinutes(15);

            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            /*Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. 
             * Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant 
             * cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            */

            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + model.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:RedirectUrl"]);

            vnpay.AddRequestData("vnp_TxnRef", $"{model.OrderId}_{tick}");
            /* Mã tham chiếu của giao dịch tại hệ thống của merchant.
             * Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY.
             * Không được trùng lặp trong ngày*/
            //Add Params of 2.1.0 Version
            vnpay.AddRequestData("vnp_ExpireDate", expiredDate.ToString("yyyyMMddHHmmss"));

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:PaymentUrl"], _config["VnPay:HashSecret"]);
            return paymentUrl;
        }

        public PaymentResponseModel MakePayment(IQueryCollection colletions)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in colletions)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            PaymentResponseModel response = new PaymentResponseModel();

            var orderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            var txnRef = vnpay.GetResponseData("vnp_TxnRef");
            var transactionId = vnpay.GetResponseData("vnp_TransactionNo");
            var responseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var secureHash = colletions.FirstOrDefault(p => p.Key.Equals("vnp_SecureHash")).Value;

            var amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount"));
            var transactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");

            bool checkSignature = vnpay.ValidateSignature(secureHash, _config["VnPay:HashSecret"]);

            if (!checkSignature)
            {
                return new PaymentResponseModel()
                {
                    Success = false
                };
            }

            return new PaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = orderInfo,
                OrderId = txnRef,
                TransactionId = transactionId,
                Token = secureHash,
                VnPayResponseCode = responseCode,
            };
        }

    }
}
