using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TUQA_Shop.DTOs;
using TUQA_Shop.models;
using Mapster;
using TUQA_Shop.Services;

namespace TUQA_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {

        private readonly IBrandService brandService;

        public BrandsController(IBrandService brandService)
        {
            this.brandService = brandService;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var brands = brandService.GetAll();
            return Ok(brands.Adapt<IEnumerable<BrandResponse>>());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var brand = brandService.Get(e => e.Id == id);
            return (brand is null) ? NotFound() : Ok(brand.Adapt<BrandResponse>());
        }

        [HttpPost("")]
        public IActionResult Creat([FromBody] BrandRequest brand)
        {
            var NewBrand = brandService.Add(brand.Adapt<Brand>());

            return CreatedAtAction(nameof(GetById), new { NewBrand.Id }, NewBrand);
        }

        [HttpPut("")]
        public IActionResult Update([FromRoute] int id, [FromBody] BrandRequest brand)
        {
            var newBrand = brandService.update(id, brand.Adapt<Brand>());
            if (newBrand == false)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("")]
        public IActionResult Delete([FromRoute] int id)
        {
            var brand = brandService.Delete(id);
            if (brand == false)
                return NotFound();
            return NoContent();
        }
    
}
}
