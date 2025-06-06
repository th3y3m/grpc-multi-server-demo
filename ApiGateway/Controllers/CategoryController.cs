using ApiGateway.Business.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiGateway.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryBusiness _categoryBusiness;

        public CategoryController(ICategoryBusiness categoryBusiness)
        {
            _categoryBusiness = categoryBusiness;
        }

        [EnableRateLimiting("fixed")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryBusiness.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = "Category not found" });
            }
            return Ok(category);
        }
    }
}
