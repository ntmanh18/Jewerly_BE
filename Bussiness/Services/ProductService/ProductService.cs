using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.ProductGemService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.ProductGemModel;
using Data.Model.ProductModel;
using Data.Model.ResultModel;
using Data.Repository.GemRepo;
using Data.Repository.GoldRepo;
using Data.Repository.ProductRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IGoldRepo _goldRepo;
        private readonly IGemRepo _gemRepo;
        private readonly IProductGemService _productGemService;


        public ProductService(IProductRepo productRepo, IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService,
            IGoldRepo goldRepo,
            IGemRepo gemRepo,
            IProductGemService productGemService)    
        {
            
            _goldRepo = goldRepo;
            _productRepo = productRepo;
            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
            _gemRepo = gemRepo;
            _productGemService = productGemService;
            
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
        private async Task<string> GenerateId(string? category, string? material,string name)
        {
            var id = "";
            string cate = "";
            string ma = "";
            string na = "";
            if (!string.IsNullOrEmpty(category))
            {
                char[] categoryList = category.ToCharArray();
                cate = categoryList[0].ToString() + categoryList[1].ToString();
            }
            
            if (!string.IsNullOrEmpty(material))
            {
                string[] materialList = material.Split(" ");
                foreach(var c in materialList)
                {
                    ma =ma + c.ToCharArray()[0];
                }
            }

            string[] nameList = name.Split(" ");
            foreach (var c in nameList)
            {
                if(!string.IsNullOrWhiteSpace(c))
                {
                    na = na + c.ToCharArray()[0];
                }
               
            }

            List<Product> productList = await _productRepo.GetAllProductsv2();
            int length = productList.Count() + 1;
            id = cate.ToUpper() + ma.ToUpper() + na.ToUpper() + length.ToString();


            return id;
        }
        static bool IsPositiveRealNumber(string input)
        {
            // Regular expression pattern to match positive real numbers
            string pattern = @"^\d*\.?\d+$";

            // Match the input string against the pattern
            return Regex.IsMatch(input, pattern);
        }


        public async Task<ResultModel> CreateProduct(string token, CreateProductReqModel productModel)
        {

            var res = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };
            

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2});
            if (!isValidRole)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "You don't permission to perform this action.";

                return res;
            }
            if(!(IsPositiveRealNumber(productModel.Weight.ToString()) ||
                 IsPositiveRealNumber(productModel.MachiningCost.ToString()) ||
                 IsPositiveRealNumber(productModel.Size.ToString())
                ))
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Weight, Cost and Size must be a positive number";
                return res;
            }
            
            Gold material = await _goldRepo.GetGoldById(productModel.Material);
             if (material == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Material is not existed ";
                return res;
            }
           
            
            string id = await GenerateId(productModel.Category,material.GoldName,  productModel.ProductName);
            
            Product p = new Product(){
                ProductId = id,
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
            
            await _productRepo.Insert(p);
            ProductGemReqModel model = new ProductGemReqModel() { 
            ProductId=id,
            Gem = productModel.Gem,
            };
            await _productGemService.CreateProductGem(token, model);
           
            
            res.IsSuccess = true;
            res.Code = (int)HttpStatusCode.OK;
            res.Data = productModel;
            return res;


        }
    }
}
