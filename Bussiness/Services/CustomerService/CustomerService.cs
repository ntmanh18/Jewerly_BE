using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.CustomerModel;
using Data.Model.ResultModel;
using Data.Repository.CustomerRepo;
using Data.Repository.UserRepo;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bussiness.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private static Random _random = new Random();
        private readonly ICustomerRepo _customerRepo;
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authentocateService;
        private readonly IToken _token;

        public CustomerService(
            ICustomerRepo customerRepo,
            IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService
            )
        {
            

            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
            _customerRepo = customerRepo;

        }
        public async Task<ResultModel> CreateCustomer(string? token, CustomerCreateModel customerModel)
        {
            


            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 1, 2, 3 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }

            try
            {
                if (!IsValid(customerModel.Email))
                {
                    resultModel.IsSuccess = false;
                    resultModel.Code = 400;
                    resultModel.Message = "Wrong Email Type";

                }
                else if (!IsPhoneNumber(customerModel.Phone))
                {
                    resultModel.IsSuccess = false;
                    resultModel.Code = 400;
                    resultModel.Message = "Wrong Phone Type";

                }
                else
                {//nghiên cứu auto-mapper
                    CustomerRequestModel customerRequestModel = new CustomerRequestModel
                    {
                        FullName = customerModel.FullName,
                        DoB = DateOnly.FromDateTime(customerModel.DoB),
                        Address = customerModel.Address,
                        Email = customerModel.Email,
                        Phone = customerModel.Phone,
                        Point = 0,
                        Rate = "Đồng",
                    };


                    Customer customer = new Customer
                    {
                        CustomerId = GenerateCustomerId(),
                        FullName = customerRequestModel.FullName,
                        DoB = customerRequestModel.DoB,
                        Address = customerRequestModel.Address,
                        Email = customerRequestModel.Email,
                        Phone = customerRequestModel.Phone,
                        Point = customerRequestModel.Point,
                        Rate = customerRequestModel.Rate,
                    };
                    await _customerRepo.CreateCustomer(customer);
                    resultModel.IsSuccess = true;
                    resultModel.Code = 200;
                    resultModel.Data = customer;
                    resultModel.Message = "Correct";
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return resultModel;
        }
        private static bool IsValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^([0-9]{10})$").Success;
        }
        public static string GenerateCustomerId()
        {

            int randomNumber = _random.Next(0, 10000);
            string numberPart = randomNumber.ToString("D5");
            return "C" + numberPart;
        }
    }
}
