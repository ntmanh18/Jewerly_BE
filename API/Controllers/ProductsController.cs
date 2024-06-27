using Bussiness.Services.ProductService;
using Data.Entities;
using Data.Model.ProductGemModel;
using Data.Model.ProductModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-products")]
        public async Task<ActionResult> GetProducts()
        {
            


            var result = await _productService.GetProducts();
            return Ok(result.ToList());
        }

        [HttpGet("Get-Products-By-Name")]
        public async Task<ActionResult> GetProductsByName([FromQuery] string? searchProductName)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _productService.GetProductsByName(token, searchProductName);
            return StatusCode(res.Code, res);
            //var products = await _productService.GetProductsByName(searchProductName);
            //if (products == null)
            //{
            //    return NotFound("Product not found");
            //}
            //return Ok(products);
        }

        [HttpGet("Get-By-Id")]
        public async Task<ActionResult<Product>> GetById(string productId)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _productService.GetProductById(token, productId);
            return StatusCode(res.Code, res);
            //var product = await _productService.GetProductById(productId);
            //if (product == null)
            //{
            //    return NotFound("Product not found");
            //}

            //return Ok(product);
        }

        //[HttpPut("{productId}")]
        [HttpPut("product-Update")]
        public async Task<ActionResult> UpdateProduct(ProductRequestModel productUpdate)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _productService.UpdateProduct(token, productUpdate);
            return StatusCode(res.Code, res);
            //var result = await _productService.UpdateProduct(productUpdate);
            //return Ok(result);
        }
        [HttpPost]
        [Route("create-product")]
        public async Task<ActionResult> Create([FromBody]CreateProductReqModel product)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _productService.CreateProduct(token, product);
            return StatusCode(res.Code, res);
        }

        [HttpGet]
        [Route("view-product")] 
        public async Task<ActionResult> ViewProductV2([FromQuery]ProductQueryObject product)
        {
            string? token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var res = await _productService.GetAllProductv2(token, product);
            return StatusCode(res.Code, res);
        }
    }
}
