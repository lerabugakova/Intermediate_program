using CatalogService.Models;

namespace CatalogService.Services
{
    public class CatalogRepository
    {
        private List<Category> _categories = new();
        private List<Item> _items = new();
        private int _categoryId = 1;
        private int _itemId = 1;

        public IEnumerable<Category> GetCategories() => _categories;
        public IEnumerable<Item> GetItems(int? categoryId, int page, int pageSize)
        {
            var query = _items.AsQueryable();
            if (categoryId.HasValue)
                query = query.Where(i => i.CategoryId == categoryId.Value);
            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public Category AddCategory(string name)
        {
            var category = new Category { Id = _categoryId++, Name = name };
            _categories.Add(category);
            return category;
        }

        public Item AddItem(string name, int categoryId)
        {
            var item = new Item { Id = _itemId++, Name = name, CategoryId = categoryId };
            _items.Add(item);
            return item;
        }

        public bool UpdateCategory(int id, string name)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return false;
            category.Name = name;
            return true;
        }

        public bool UpdateItem(int id, string name, int categoryId)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null) return false;
            item.Name = name;
            item.CategoryId = categoryId;
            return true;
        }

        public bool DeleteCategory(int id)
        {
            _items.RemoveAll(i => i.CategoryId == id);
            return _categories.RemoveAll(c => c.Id == id) > 0;
        }

        public bool DeleteItem(int id) => _items.RemoveAll(i => i.Id == id) > 0;
    }
}
