
using YumBlazor.Models;
using YumBlazor.Repository;

namespace YumBlazor.Tests
{
    public class InMemoryRepositoryTest
    {
        private readonly InMemoryRepository<Category> _repo;
        private readonly List<Category> _categories = new List<Category>();
        public InMemoryRepositoryTest()
        {
            _categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Appetizer" },
                new Category { Id = Guid.NewGuid(), Name = "Entree" },
                new Category { Id = Guid.NewGuid(), Name = "Dessert" }
            };  
            _repo = new InMemoryRepository<Category>();
            foreach (var category in _categories)
            {
                _repo.AddAsync(category).Wait();

            }
        }  
        
        [Fact]
        public async Task GetAllAsync()
        {


            // Act
            var categories = await _repo.GetAllAsync();

            // Assert
            Assert.Equal(3, categories.Count());
            Assert.Contains(categories, c => c.Name == "Appetizer");
            Assert.Contains(categories, c => c.Name == "Entree");
            Assert.Contains(categories, c => c.Name == "Dessert");
        }

        [Fact]
        public async Task GetById()
        {
            var id = _categories.FirstOrDefault(c => c.Name == "Appetizer")?.Id ?? Guid.Empty;

            // Act
            var category = await _repo.GetByIdAsync(id);

            // Assert
            Assert.NotNull(category);
            Assert.Equal("Appetizer", category!.Name);

        }

        [Fact]
        public async Task DeleteById()
        {


            // Act
            var id = _categories.FirstOrDefault(c => c.Name == "Appetizer").Id;  
            await _repo.DeleteAsync(id);
            var categories = await _repo.GetAllAsync();

            // Assert
            Assert.Equal(2, categories.Count());
            Assert.Contains(categories, c => c.Name == "Entree");
            Assert.Contains(categories, c => c.Name == "Dessert");
            Assert.DoesNotContain(categories, c => c.Name == "Appetizer");

        }

        [Fact]
        public async Task DeleteByRange()
        {
            var categories = await _repo.GetAllAsync();
            var categoriesToDelete = categories.Where(c => c.Name == "Appetizer" || c.Name == "Dessert").ToList();  

            // Act
        
            await _repo.DeleteRangeAsync(categoriesToDelete);
            var categoriesAfterDeletion = await _repo.GetAllAsync();

            // Assert
            Assert.Equal(1, categoriesAfterDeletion.Count());
            Assert.Contains(categories, c => c.Name == "Entree");
            Assert.DoesNotContain(categories, c => c.Name == "Dessert");
            Assert.DoesNotContain(categories, c => c.Name == "Appetizer");

        }

        [Fact]  
        public async Task UpdateCategory()
        {
            var id = _categories.FirstOrDefault(c => c.Name == "Appetizer")?.Id ?? Guid.Empty;  
            var categoryToUpdate = await _repo.GetByIdAsync(id);
            Assert.NotNull(categoryToUpdate);
            categoryToUpdate!.Name = "Updated Appetizer";
            // Act
            await _repo.UpdateAsync(categoryToUpdate);
            var updatedCategory = await _repo.GetByIdAsync(id);
            // Assert
            Assert.NotNull(updatedCategory);
            Assert.Equal("Updated Appetizer", updatedCategory!.Name);
        }
    }
}
