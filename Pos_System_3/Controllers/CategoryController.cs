using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos_System_3.Model;
using Pos_System_3.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pos_System_3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, IUserService userService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (!IsAdminUser())
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            await _categoryService.AddCategoryAsync(category);
            return Ok(new { message = "Category added successfully" });
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (!IsAdminUser())
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var success = await _categoryService.UpdateCategoryAsync(id, category.Name);
            if (!success)
            {
                return NotFound(new { message = "Category not found" });
            }

            return Ok(new { message = "Category updated successfully" });
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (!IsAdminUser())
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var success = await _categoryService.RemoveCategoryAsync(id);
            if (!success)
            {
                return NotFound(new { message = "Category not found" });
            }

            return Ok(new { message = "Category deleted successfully" });
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategory(int id)
        {
            if (!IsAdminUser())
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var category = await _categoryService.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            return Ok(category);
        }

        [HttpGet("getall")]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            if (!IsAdminUser())
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        private bool IsAdminUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (userId == null)
            {
                _logger.LogError("UserID is null in Category");
                return false;
            }

            var user = _userService.GetUserByIdAsync(int.Parse(userId)).Result;
            return user != null && user.UserRole == UserRole.Admin;
        }
    }
}
