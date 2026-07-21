using BuildHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildHub.Controllers
{
    public class CategoryController : ControllerBase
    {
        private CategoryService categoryService;
        public CategoryController (CategoryService _categoryService)
        {
            categoryService = _categoryService;
        }
    }
}
