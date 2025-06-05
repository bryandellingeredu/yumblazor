using System.ComponentModel.DataAnnotations;
using YumBlazor.Repository;

namespace YumBlazor.Models
{
    public class Product : IEntity  
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 1000.00)]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string? SpecialTag { get; set; }

        public string? ImageUrl { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }   
    }
}
