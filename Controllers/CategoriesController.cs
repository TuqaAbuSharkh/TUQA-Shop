using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TUQA_Shop.models;
using TUQA_Shop.Services;

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
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = categoryService.Get(e => e.Id == id);
            return (category is null) ? NotFound() : Ok(category);
        }

        [HttpPost("")]
        public IActionResult Creat([FromBody]Category category)
        {
            var NewCategory = categoryService.Add(category);

            return CreatedAtAction(nameof(GetById), new { NewCategory.Id }, NewCategory);
        }

        [HttpPut("")]
        public IActionResult Update([FromRoute]int id, [FromBody]Category category)
        {
            var newCategory = categoryService.update(id, category);
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
