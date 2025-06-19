using CatalogService.DTOs;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly CatalogRepository _repo;

        public ItemsController(CatalogRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult Get([FromQuery] int? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var items = _repo.GetItems(categoryId, page, pageSize);
            return Ok(items);
        }

        [HttpPost]
        public IActionResult Create(ItemDto dto)
        {
            var item = _repo.AddItem(dto.Name, dto.CategoryId);
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ItemDto dto)
        {
            if (!_repo.UpdateItem(id, dto.Name, dto.CategoryId))
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_repo.DeleteItem(id))
                return NotFound();
            return NoContent();
        }
    }

}
