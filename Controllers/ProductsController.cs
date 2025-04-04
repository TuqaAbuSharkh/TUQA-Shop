using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TUQA_Shop.DTOs;
using TUQA_Shop.Services;
using Mapster;
using TUQA_Shop.models;


namespace TUQA_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }


        [HttpGet("")]
        public IActionResult GetAll()
        {
            var products = productService.GetAll();
            return Ok(products.Adapt<IEnumerable<ProductResponse>>());
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = productService.Get(e => e.Id == id);
            return (product is null) ? NotFound() : Ok(product.Adapt<ProductResponse>());
        }

        [HttpPost("")]
        public IActionResult Creat([FromForm] ProductRequest productRequest)
        {
            var file = productRequest.mainImg;
            var product = productRequest.Adapt<Product>();
            if (file!= null && file.Length > 0)
            {
                var NewProduct = productService.Add(product);
                return CreatedAtAction(nameof(GetById), new { NewProduct.Id }, NewProduct);

            }
            return BadRequest();
        }


        [HttpDelete("")]
        public IActionResult Delete([FromRoute] int id)
        {
            var product = productService.Delete(id);
            if (product == false)
                return NotFound();
            return NoContent();
        }
    }
}
