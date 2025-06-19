using CatalogService.DTOs;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly CatalogRepository _repo;

        public CategoriesController(CatalogRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult Get() => Ok(_repo.GetCategories());

        [HttpPost]
        public IActionResult Create(CategoryDto dto)
        {
            var category = _repo.AddCategory(dto.Name);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryDto dto)
        {
            if (!_repo.UpdateCategory(id, dto.Name))
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_repo.DeleteCategory(id))
                return NotFound();
            return NoContent();
        }
    }

}
