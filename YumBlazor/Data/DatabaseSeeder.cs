using Microsoft.AspNetCore.Identity;
using YumBlazor.Models;
using YumBlazor.Utility;

namespace YumBlazor.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category { Name = "Appetizer" });
                context.Categories.Add(new Category { Name = "Entree" });
                context.Categories.Add(new Category { Name = "Dessert" });
                await context.SaveChangesAsync();
            }
            if (!context.Products.Any())
            {
                var appetizerCategory = context.Categories.First(c => c.Name == "Appetizer");
                var entreeCategory = context.Categories.First(c => c.Name == "Entree");
                var dessertCategory = context.Categories.First(c => c.Name == "Dessert");

                context.Products.AddRange(
                    new Product
                    {
                        Name = "Samosa",
                        Price = 2.99M,
                        Description = "Crispy pastry filled with spicy potatoes and peas.",
                        SpecialTag = "Vegetarian",
                        ImageUrl = "/lib/images/product/06a7abb7-1327-41bc-b379-8834c73b9160.jpg",
                        CategoryId = appetizerCategory.Id
                    },
                    new Product
                    {
                        Name = "Gulab Jamun",
                        Price = 3.49M,
                        Description = "Milk-solid balls soaked in rose-flavored sugar syrup.",
                        SpecialTag = "Sweet",
                        ImageUrl = "/lib/images/product/22aa1c1c-d0d0-4bdf-814a-b56a2aeb2aee.jpg",
                        CategoryId = dessertCategory.Id
                    },
                    new Product
                    {
                        Name = "Jalebi",
                        Price = 2.99M,
                        Description = "Crispy, sugary spirals soaked in syrup.",
                        SpecialTag = "Sweet",
                        ImageUrl = "/lib/images/product/49f1cf5b-e535-487a-a6df-29cad827a9c7.jpg",
                        CategoryId = dessertCategory.Id
                    },
                    new Product
                    {
                        Name = "Mango Lassi",
                        Price = 2.49M,
                        Description = "Refreshing mango and yogurt smoothie.",
                        SpecialTag = "Drink",
                        ImageUrl = "/lib/images/product/75b8200b-682c-4304-86ee-86bf5ade95d4.jpg",
                        CategoryId = appetizerCategory.Id
                    },
                    new Product
                    {
                        Name = "Dry Fruit Laddu",
                        Price = 4.99M,
                        Description = "Energy-rich snack balls made from dried fruits and nuts.",
                        SpecialTag = "Healthy",
                        ImageUrl = "/lib/images/product/487c4c75-a4fe-4824-9bc4-956e05bce613.jpg",
                        CategoryId = dessertCategory.Id
                    },
                    new Product
                    {
                        Name = "Samosa (2nd)",
                        Price = 2.99M,
                        Description = "Classic Indian snack filled with spiced vegetables.",
                        SpecialTag = "Spicy",
                        ImageUrl = "/lib/images/product/58981d17-65e8-4448-a6ae-c4f712f31fe0.jpg",
                        CategoryId = appetizerCategory.Id
                    },
                    new Product
                    {
                        Name = "Paneer Butter Masala",
                        Price = 10.99M,
                        Description = "Cottage cheese cubes in creamy tomato sauce.",
                        SpecialTag = "Vegetarian",
                        ImageUrl = "/lib/images/product/d724da18-d558-4f95-bfbd-489612a56cb5.jpg",
                        CategoryId = entreeCategory.Id
                    },
                    new Product
                    {
                        Name = "Spring Rolls",
                        Price = 5.99M,
                        Description = "Crispy rolls with mixed veggie stuffing.",
                        SpecialTag = "Asian",
                        ImageUrl = "/lib/images/product/df7ed93c-a1b2-446e-be37-e1b67af9424b.jpg",
                        CategoryId = appetizerCategory.Id
                    },
                    new Product
                    {
                        Name = "Pav Bhaji",
                        Price = 6.99M,
                        Description = "Spiced mashed vegetables with buttered bread.",
                        SpecialTag = "Street Food",
                        ImageUrl = "/lib/images/product/e89cfdea-6212-4a7f-a01b-4cbb276ae03c.jpg",
                        CategoryId = entreeCategory.Id
                    },
                    new Product
                    {
                        Name = "Butter Chicken",
                        Price = 11.99M,
                        Description = "Marinated chicken in creamy tomato sauce.",
                        SpecialTag = "Popular",
                        ImageUrl = "/lib/images/product/eb1fca06-33e8-44fa-acbe-b906428040bb.jpg",
                        CategoryId = entreeCategory.Id
                    }
                );
                await context.SaveChangesAsync();
            }
            string[] roles = new[] { SD.Role_Admin, SD.Role_Customer}; 

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
