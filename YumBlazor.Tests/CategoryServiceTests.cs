using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YumBlazor.Models;
using YumBlazor.Repository;
using YumBlazor.Services;

namespace YumBlazor.Tests
{
    public class CategoryServiceTests : IAsyncLifetime
    {
        private readonly InMemoryRepository<Category> _repo = new();
        private CategoryService _service = default!;
        private readonly List<Category> _seed = new()
    {
        new Category { Id = Guid.NewGuid(), Name = "Appetizer" },
        new Category { Id = Guid.NewGuid(), Name = "Entree" },
        new Category { Id = Guid.NewGuid(), Name = "Dessert" }
    };

        public async Task InitializeAsync()
        {
            foreach (var c in _seed)
                await _repo.AddAsync(c);

            _service = new CategoryService(_repo);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task GetAllCategoriesAsync_ReturnsAll()
        {
            // Act
            var categories = await _service.GetAllCategoriesAsync();

            // Assert
            Assert.Equal(3, categories.Count());
            Assert.Contains(categories, c => c.Name == "Appetizer");
            Assert.Contains(categories, c => c.Name == "Entree");
            Assert.Contains(categories, c => c.Name == "Dessert");
        }
    }
}
