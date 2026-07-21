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
    }
}
