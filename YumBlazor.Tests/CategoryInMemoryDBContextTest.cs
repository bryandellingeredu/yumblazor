using Microsoft.EntityFrameworkCore;
using YumBlazor.Data;

namespace YumBlazor.Tests
{
    public class CategoryInMemoryDBContextTest
    {
        [Fact]
        public async Task AddCategory_persists_to_InMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase(databaseName: $"YumBlazor-{System.Guid.NewGuid()}")
          .Options;

            await using var ctx = new ApplicationDbContext(options);
            var id = System.Guid.NewGuid();
            var category = new Models.Category { Name = "Beverages", Id = id };
            await ctx.Categories.AddAsync(category);
            await ctx.SaveChangesAsync();
            var exists = await ctx.Categories.FindAsync(id);
            Assert.NotNull(exists);
            Assert.Equal("Beverages", exists!.Name);
        }
    }
}
