using System.ComponentModel.DataAnnotations.Schema;
using YumBlazor.Repository;

namespace YumBlazor.Models
{
    public class ShoppingCart : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserId { get; set; } = string.Empty;


        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public Guid ProductId { get; set; }


        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Count { get; set; }  

    }
    
}
