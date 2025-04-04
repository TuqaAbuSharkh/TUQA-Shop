using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TUQA_Shop.models;
using Mapster;
using TUQA_Shop.Services;
using TUQA_Shop.DTOs;

namespace TUQA_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var categories = categoryService.GetAll();
            return Ok(categories.Adapt<IEnumerable<CategoryResponse>>());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = categoryService.Get(e => e.Id == id);
            return (category is null) ? NotFound() : Ok(category.Adapt<CategoryResponse>());
        }

        [HttpPost("")]
        public IActionResult Creat([FromBody] CategoryRequest category)
        {
            var NewCategory = categoryService.Add(category.Adapt<Category>());

            return CreatedAtAction(nameof(GetById), new { NewCategory.Id }, NewCategory);
        }

        [HttpPut("")]
        public IActionResult Update([FromRoute]int id, [FromBody]CategoryRequest category)
        {
            var newCategory = categoryService.update(id, category.Adapt<Category>());
            if (newCategory == false)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("")]
        public IActionResult Delete([FromRoute]int id)
        {
            var category = categoryService.Delete(id);
            if (category == false)
                return NotFound();
            return NoContent();
        }
    }
}
