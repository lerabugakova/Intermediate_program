using CatalogService.Models;
using CatalogService.Services;
using Xunit;

public class CatalogRepositoryTests
{
    private CatalogRepository _repo;

    public CatalogRepositoryTests() => _repo = new CatalogRepository();

    [Fact]
    public void Can_Add_And_Get_Categories()
    {
        var cat = _repo.AddCategory("Books");
        var categories = _repo.GetCategories().ToList();

        Assert.Single(categories);
        Assert.Equal("Books", categories[0].Name);
    }

    [Fact]
    public void Can_Update_Category()
    {
        var cat = _repo.AddCategory("Old Name");
        var updated = _repo.UpdateCategory(cat.Id, "New Name");

        Assert.True(updated);
        Assert.Equal("New Name", _repo.GetCategories().First().Name);
    }

    [Fact]
    public void Can_Delete_Category_And_Items()
    {
        var cat = _repo.AddCategory("Temp");
        _repo.AddItem("Item 1", cat.Id);
        _repo.AddItem("Item 2", cat.Id);

        _repo.DeleteCategory(cat.Id);

        Assert.Empty(_repo.GetCategories());
        Assert.Empty(_repo.GetItems(null, 1, 10));
    }

    [Fact]
    public void Can_Add_And_Filter_Items_By_Category()
    {
        var cat1 = _repo.AddCategory("Books");
        var cat2 = _repo.AddCategory("Electronics");

        _repo.AddItem("Book 1", cat1.Id);
        _repo.AddItem("TV", cat2.Id);

        var filtered = _repo.GetItems(cat1.Id, 1, 10).ToList();
        Assert.Single(filtered);
        Assert.Equal("Book 1", filtered[0].Name);
    }

    [Fact]
    public void Can_Paginate_Items()
    {
        var cat = _repo.AddCategory("Test");
        for (int i = 0; i < 25; i++)
            _repo.AddItem($"Item {i}", cat.Id);

        var page1 = _repo.GetItems(cat.Id, 1, 10).ToList();
        var page3 = _repo.GetItems(cat.Id, 3, 10).ToList();

        Assert.Equal(10, page1.Count);
        Assert.Equal(5, page3.Count); // Last page
    }
}
