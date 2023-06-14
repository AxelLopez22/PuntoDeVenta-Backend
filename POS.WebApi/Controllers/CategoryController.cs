using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.Request;
using POS.Application.Interfaces;
using POS.Infraestructure.Commons.Bases.Request;

namespace POS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryApplication _category;

        public CategoryController(ICategoryApplication category)
        {
            _category = category;
        }

        [HttpPost]
        public async Task<IActionResult> ListCategories([FromBody] BaseFiltersRequest filters)
        {
            var response = await _category.ListCategories(filters);
            return Ok(response);
        }

        [HttpGet("select")]
        public async Task<IActionResult> ListSelectCategories()
        {
            var response = await _category.ListSelectCategories();
            return Ok(response);
        }

        [HttpGet("{categoryId:int}")]
        public async Task<IActionResult> CategoryById(int categoryId)
        {
            var response = await _category.CategoryById(categoryId);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCategory([FromBody] CategoryRequestDTO request)
        {
            var response = await _category.RegisterCategory(request);
            return Ok(response);
        }

        [HttpPut("edit/{categoryId:int}")]
        public async Task<IActionResult> EditCategory(int categoryId, [FromBody] CategoryRequestDTO request)
        {
            var response = await _category.EditCategory(categoryId, request);
            return Ok(response);
        }

        [HttpDelete("remove/{categoryId:int}")]
        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            var response = await _category.RemoveCategory(categoryId);
            return Ok(response);
        }
    }
}
