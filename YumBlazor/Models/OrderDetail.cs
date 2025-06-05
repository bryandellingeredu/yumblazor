using System.ComponentModel.DataAnnotations;
using YumBlazor.Repository;

namespace YumBlazor.Models
{
    public class OrderDetail : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid OrderHeaderId { get; set; }

        public OrderHeader OrderHeader { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public double Price { get; set; }


        [Required]
        public string ProductName { get; set; } = string.Empty;
    }
}
