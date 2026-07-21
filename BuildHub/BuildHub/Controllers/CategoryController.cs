using BuildHub.Models;
using BuildHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    [ApiController]
    [Route("category")]
    public class CategoryController : ControllerBase
    {
        private CategoryService categoryService;
        public CategoryController (CategoryService _categoryService)
        {
            categoryService = _categoryService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok (categoryService.GetAll());
        }

        [HttpGet("GetById")]
        public IActionResult GetById([FromRoute] int id)
        {
            return Ok(categoryService.GetById(id));
        }

        [HttpPost("UpdateType/{categoryId}")]
        public IActionResult UpdateType([FromRoute] int categoryId, [FromQuery] string type)
        {
            bool updated = categoryService.UpdateType(categoryId, type);

            if (!updated)
                return NotFound();
            return Ok("Updated succesfully");
        }

        [HttpDelete("Delete/{categoryId}")]
        public IActionResult Delete([FromRoute] int categoryId)
        {
            bool deleted = categoryService.Delete(categoryId);

            if (!deleted)
                return NotFound();
            return Ok ("deleted succesfully");
        }
    }
}
