using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TUQA_Shop.models;
using Mapster;
using TUQA_Shop.Services;
using TUQA_Shop.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TUQA_Shop.Utility;

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
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryService.GetAsync();
            return Ok(categories.Adapt<IEnumerable<CategoryResponse>>());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await categoryService.GetOne(e => e.Id == id);
            return (category is null) ? NotFound() : Ok(category.Adapt<CategoryResponse>());
        }

        [HttpPost("")]
        [Authorize($"{StaticData.SuperAdmin},{StaticData.Admin},{StaticData.Company}")]
        public async Task<IActionResult> Creat([FromBody] CategoryRequest category)
        {
            var NewCategory =await  categoryService.AddAsync(category.Adapt<Category>());

            return CreatedAtAction(nameof(GetById), new { NewCategory.Id }, NewCategory);
        }

        [HttpPut("")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]CategoryRequest category)
        {
            var newCategory =await categoryService.updateAsync(id, category.Adapt<Category>());
            if (newCategory == false)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var category =await  categoryService.RemoveAsync(id);
            if (category == false)
                return NotFound();
            return NoContent();
        }
    }
}
