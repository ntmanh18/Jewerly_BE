using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.ProductModel;
using Data.Model.ResultModel;
using Data.Repository.ProductRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bussiness.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authentocateService;
        private readonly IToken _token;

        public ProductService(IProductRepo productRepo, IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService)
        {
            
            _productRepo = productRepo;
            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
        }
        

        public async Task<IEnumerable<ProductRequestModel>> GetProducts()
        {

            var products = await _productRepo.GetProducts();
            List<ProductRequestModel> updatedProducts = new List<ProductRequestModel>();

            foreach (var product in products)
            {
                ProductRequestModel product1 = new ProductRequestModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Category = product.Category,
                    Material = product.Material,
                    Weight = product.Weight,
                    MachiningCost = product.MachiningCost,
                    Size = product.Size,
                    Amount = product.Amount,
                    Desc = product.Desc,
                    Image = product.Image,
                };
                updatedProducts.Add(product1);

            }
            
            return updatedProducts;
        }

        public async Task<ResultModel> GetProductsByName(string? token, string name)
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
            IEnumerable<Product> products = await _productRepo.GetProducts();
            List<ProductRequestModel> updatedProducts = new List<ProductRequestModel>();
            foreach (var product in products)
            {
                ProductRequestModel product1 = new ProductRequestModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Category = product.Category,
                    Material = product.Material,
                    Weight = product.Weight,
                    MachiningCost = product.MachiningCost,
                    Size = product.Size,
                    Amount = product.Amount,
                    Desc = product.Desc,
                    Image = product.Image,
                };
                updatedProducts.Add(product1);

            }

            IEnumerable<ProductRequestModel> updatedProducts2 = updatedProducts;


            
            if (String.IsNullOrEmpty(name))
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Please enter any product name";
                resultModel.Data = updatedProducts2;
            }
            else
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                updatedProducts2 = updatedProducts2.Where(p => RemoveDiacritics(p.ProductName).ToLower().Contains(RemoveDiacritics(name).ToLower()));
                if (updatedProducts2.Count() > 0)
                {
                    resultModel.Message = $"Success - There are {products.Count()} found";
                }
                else
                {
                    resultModel.Message = "Not found";
                }
                
                resultModel.Data = updatedProducts2;
            }
            return resultModel;
        }

        public async Task<ResultModel> GetProductById(string? token, string productId)
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
            IEnumerable<Product> products = await _productRepo.GetProducts();
            List<ProductRequestModel> updatedProducts = new List<ProductRequestModel>();
            foreach (var product in products)
            {
                ProductRequestModel product1 = new ProductRequestModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Category = product.Category,
                    Material = product.Material,
                    Weight = product.Weight,
                    MachiningCost = product.MachiningCost,
                    Size = product.Size,
                    Amount = product.Amount,
                    Desc = product.Desc,
                    Image = product.Image,
                };
                updatedProducts.Add(product1);

            }
            
            if (String.IsNullOrEmpty(productId))
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Please enter any product Id";

            }
            else
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;

                for (int i = 0; i < updatedProducts.Count; i++)
                {
                    if (updatedProducts[i].ProductId != productId)
                    {
                        updatedProducts.RemoveAll(p => p.ProductId == updatedProducts[i].ProductId);
                        i = i - 1;
                    }

                }
                if (updatedProducts.Count() > 0)
                {
                    resultModel.Message = "Product found";
                }
                else
                {
                    resultModel.Message = "Not found";
                }
                IEnumerable<ProductRequestModel> updatedProducts2 = updatedProducts;
                resultModel.Data = updatedProducts2;
            }
            return resultModel;

            }

        public async Task<ResultModel> UpdateProduct(string? token, ProductRequestModel productModel)
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
            var existingProduct = await GetProductById(token, productModel.ProductId);

            if (existingProduct.Message == "Not found")
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Request product not found";
            }
            else if (existingProduct.Message == "Product found")
            {
                if (!IsNumber(productModel.Material) || !IsNumber(productModel.Weight.ToString()) || !IsNumber(productModel.MachiningCost.ToString()) || !IsNumber(productModel.Size.ToString()) || !IsNumber(productModel.Amount.ToString()))
                {
                    resultModel.Message = "Wrong Type - Must be number";
                    resultModel.Code = 400;
                    resultModel.IsSuccess = false;
                }
                else
                {
                    resultModel.Code = 200;
                    resultModel.IsSuccess = true;
                    resultModel.Message = "Update success";
                    Product product = new Product
                    {
                        ProductId = productModel.ProductId,
                        ProductName = productModel.ProductName,
                        Category = productModel.Category,
                        Material = productModel.Material,
                        Weight = productModel.Weight,
                        MachiningCost = productModel.MachiningCost,
                        Size = productModel.Size,
                        Amount = productModel.Amount,
                        Desc = productModel.Desc,
                        Image = productModel.Image,
                    };
                    var productUpdate = await _productRepo.UpdateProduct(product);

                    resultModel.Data = productUpdate;
                }
            }
            return resultModel;
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
        public static bool IsNumber(string input)
        {
            return Regex.IsMatch(input, @"^\d+(\.\d+)?$");
        }
    }
}
