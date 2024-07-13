using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.CustomerModel;
using Data.Model.ProductModel;
using Data.Model.ResultModel;
using Data.Repository.CustomerRepo;
using Data.Repository.UserRepo;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                //nghiên cứu auto-mapper
                {
                    CustomerRequestModel customerRequestModel = new CustomerRequestModel
                    {
                        FullName = customerModel.FullName,
                        DoB = DateOnly.FromDateTime(customerModel.DoB),
                        Address = customerModel.Address,
                        Email = customerModel.Email,
                        Phone = customerModel.Phone,
                        Point = 0,
                        Rate = "Đồng",
                        Status = customerModel.Status,
                    };


                    Customer customer = new Customer
                    {
                        CustomerId = await GenerateCustomerId(),
                        FullName = customerRequestModel.FullName,
                        DoB = customerRequestModel.DoB,
                        Address = customerRequestModel.Address,
                        Email = customerRequestModel.Email,
                        Phone = customerRequestModel.Phone,
                        Point = customerRequestModel.Point,
                        Rate = customerRequestModel.Rate,
                        Status = customerRequestModel.Status,
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

        public async Task<IEnumerable<CustomerViewModel>> GetCustomers()
        {

            var customers = await _customerRepo.GetCustomers();
            List<CustomerViewModel> updatedCustomers = new List<CustomerViewModel>();

            foreach (var customer in customers)
            {
                CustomerViewModel customer1 = new CustomerViewModel
                {
                    CustomerId = customer.CustomerId,
                    FullName = customer.FullName,
                    DoB = customer.DoB,
                    Address = customer.Address,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Point = customer.Point,
                    Rate = customer.Rate,
                    Status= customer.Status,
                    Bills = customer.Bills.Select(b => new Bill
                    {
                        BillId = b.BillId,
                        TotalCost = b.TotalCost,
                        PublishDay = b.PublishDay,
                    }).ToList(),
                };
                updatedCustomers.Add(customer1);

            }

            return updatedCustomers;
        }

        public async Task<ResultModel> GetCustomersByName(string? token, string name)
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
            //var products = await _productRepo.GetProducts();
            IEnumerable<Customer> customers = await _customerRepo.GetCustomers();
            List<CustomerViewModel> updatedCustomers = new List<CustomerViewModel>();
            foreach (var customer in customers)
            {
                CustomerViewModel customer1 = new CustomerViewModel
                {
                    CustomerId = customer.CustomerId,
                    FullName = customer.FullName,
                    DoB = customer.DoB,
                    Address = customer.Address,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Point = customer.Point,
                    Rate = customer.Rate,
                    Status = customer.Status,
                    Bills = customer.Bills.Select(b => new Bill
                    {
                        BillId = b.BillId,
                        TotalCost = b.TotalCost,
                        PublishDay = b.PublishDay,
                    }).ToList(),
                };
                updatedCustomers.Add(customer1);

            }

            IEnumerable<CustomerViewModel> updatedCustomers2 = updatedCustomers;



            if (String.IsNullOrEmpty(name))
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Please enter any product name";
                resultModel.Data = updatedCustomers2;
            }
            else
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                updatedCustomers2 = updatedCustomers2.Where(c => RemoveDiacritics(c.FullName).ToLower().Contains(RemoveDiacritics(name).ToLower()));
                if (updatedCustomers2.Count() > 0)
                {
                    resultModel.Message = $"Success - There are {customers.Count()} found";
                    resultModel.Message = $"Success - There are {updatedCustomers2.Count()} found";
                }
                else
                {
                    resultModel.Message = "Not found";
                }

                resultModel.Data = updatedCustomers2;
            }
            return resultModel;
        }

        public async Task<ResultModel> GetCustomerByPhone(string? token, string phone)
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

            IEnumerable<Customer> customers = await _customerRepo.GetCustomers();
            List<CustomerViewModel> updatedCustomers = new List<CustomerViewModel>();
            foreach (var customer in customers)
            {
                CustomerViewModel customer1 = new CustomerViewModel
                {
                    CustomerId = customer.CustomerId,
                    FullName = customer.FullName,
                    DoB = customer.DoB,
                    Address = customer.Address,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Point = customer.Point,
                    Rate = customer.Rate,
                    Status = customer.Status,
                    Bills = customer.Bills.Select(b => new Bill
                    {
                        BillId = b.BillId,
                        TotalCost = b.TotalCost,
                        PublishDay = b.PublishDay,
                    }).ToList(),
                };
                updatedCustomers.Add(customer1);

            }

            if (String.IsNullOrEmpty(phone))
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Please enter any phone number";

            }
            else if(!IsPhoneNumber(phone)){
                resultModel.IsSuccess = false;
                resultModel.Code = 400;
                resultModel.Message = "Wrong Phone Type";
            }
            else
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;

                for (int i = 0; i < updatedCustomers.Count; i++)
                {
                    if (updatedCustomers[i].Phone != phone)
                    {
                        updatedCustomers.RemoveAll(c => c.Phone == updatedCustomers[i].Phone);
                        i = i - 1;
                    }

                }
                if (updatedCustomers.Count() > 0)
                {
                    resultModel.Message = "Product found";
                }
                else
                {
                    resultModel.Message = "Not found";
                }
                IEnumerable<CustomerViewModel> updatedCustomers2 = updatedCustomers;
                resultModel.Data = updatedCustomers2;
            }
            return resultModel;

        }
        public async Task<ResultModel> GetCustomerById(string? token, string id)
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

            IEnumerable<Customer> customers = await _customerRepo.GetCustomers();
            List<CustomerViewModel> updatedCustomers = new List<CustomerViewModel>();
            foreach (var customer in customers)
            {
                CustomerViewModel customer1 = new CustomerViewModel
                {
                    CustomerId = customer.CustomerId,
                    FullName = customer.FullName,
                    DoB = customer.DoB,
                    Address = customer.Address,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Point = customer.Point,
                    Rate = customer.Rate,
                    Status = customer.Status,
                };
                updatedCustomers.Add(customer1);

            }

            if (String.IsNullOrEmpty(id))
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Please enter any phone number";

            }
            
            else
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;

                for (int i = 0; i < updatedCustomers.Count; i++)
                {
                    if (updatedCustomers[i].CustomerId != id)
                    {
                        updatedCustomers.RemoveAll(c => c.CustomerId == updatedCustomers[i].CustomerId);
                        i = i - 1;
                    }

                }
                if (updatedCustomers.Count() > 0)
                {
                    resultModel.Message = "Product found";
                }
                else
                {
                    resultModel.Message = "Not found";
                }
                IEnumerable<CustomerViewModel> updatedCustomers2 = updatedCustomers;
                resultModel.Data = updatedCustomers2;
            }
            return resultModel;

        }

        public async Task<ResultModel> UpdateCustomer(string? token, CustomerUpdateModel customerModel)
        {
            bool? status = GetCustomerById(customerModel.CustomerId).Result.Status;
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            var existingProduct = await GetCustomerById(token, customerModel.CustomerId);

            if (existingProduct.Message == "Not found")
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Request product not found";
            }
            else if (existingProduct.Message == "Product found")
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
                {

                
                resultModel.Code = 200;
                    resultModel.IsSuccess = true;
                    resultModel.Message = "Update success";
                    Customer customer = new Customer
                    {
                        CustomerId = customerModel.CustomerId,
                        FullName = customerModel.FullName,
                        DoB = DateOnly.FromDateTime(customerModel.DoB),
                        Address = customerModel.Address,
                        Email = customerModel.Email,
                        Phone = customerModel.Phone,
                        Point = customerModel.Point,
                        Rate = customerModel.Rate,
                        Status = status
                    };
                    var productUpdate = await _customerRepo.UpdateCustomer(customer);

                    resultModel.Data = productUpdate;
                }
            }
            return resultModel;
        }

        public async Task<ResultModel> DeactiveCustomer(string? token, string id)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            var existingProduct = await _customerRepo.GetCustomerById(id);

            if (existingProduct == null)
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Request customer not found";
            }
            else if(existingProduct.Status == false){
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Customer almost deactived";
            }
            else
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "deactive success";
                Customer customer = new Customer
                {
                    CustomerId = existingProduct.CustomerId,
                    FullName = existingProduct.FullName,
                    DoB = existingProduct.DoB,
                    Address = existingProduct.Address,
                    Email = existingProduct.Email,
                    Phone = existingProduct.Phone,
                    Point = existingProduct.Point,
                    Rate = existingProduct.Rate,
                    Status = false
                };
                var productUpdate = await _customerRepo.UpdateCustomer(customer);

                

            }
            return resultModel;
        }

        public async Task<Customer> GetCustomerById(string customerId)
        {
            return await _customerRepo.GetCustomerById(customerId);
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
        //public static string GenerateCustomerId()
        //{

        //    int randomNumber = _random.Next(0, 10000);
        //    string numberPart = randomNumber.ToString("D5");
        //    return "C" + numberPart;
        //}

        private async Task<string> GenerateCustomerId()
        {
            var existingIds = await _customerRepo.GetCustomers();
            HashSet<string> existingIdSet = new HashSet<string>(existingIds.Select(op => op.CustomerId));

            string newItem;
            do
            {
                int randomNumber = _random.Next(1, 100);
                string numberPart = randomNumber.ToString("D3");
                newItem = "C" + numberPart;
            } while (existingIdSet.Contains(newItem));

            return newItem;
        }

        private string RemoveDiacritics(string text)
        {
            var stringBuilder = new StringBuilder();
            try
            {
                var normalizedString = text.Normalize(NormalizationForm.FormD);


                foreach (var c in normalizedString)
                {
                    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public async Task<ResultModel> UpdateStatusCustomer(string? token, string id)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            var existingProduct = await _customerRepo.GetCustomerById(id);

            if (existingProduct == null)
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Request customer not found";
            }
            else if (existingProduct.Status == false)
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Status setted to true";
                Customer customer = new Customer
                {
                    CustomerId = existingProduct.CustomerId,
                    FullName = existingProduct.FullName,
                    DoB = existingProduct.DoB,
                    Address = existingProduct.Address,
                    Email = existingProduct.Email,
                    Phone = existingProduct.Phone,
                    Point = existingProduct.Point,
                    Rate = existingProduct.Rate,
                    Status = true
                };
                var productUpdate = await _customerRepo.UpdateCustomer(customer);

                
            }
            else
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Status setted to flase";
                Customer customer = new Customer
                {
                    CustomerId = existingProduct.CustomerId,
                    FullName = existingProduct.FullName,
                    DoB = existingProduct.DoB,
                    Address = existingProduct.Address,
                    Email = existingProduct.Email,
                    Phone = existingProduct.Phone,
                    Point = existingProduct.Point,
                    Rate = existingProduct.Rate,
                    Status = false
                };
                var productUpdate = await _customerRepo.UpdateCustomer(customer);

                

            }
            return resultModel;
        }
    }
}
