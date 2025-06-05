using System.ComponentModel.DataAnnotations;
using YumBlazor.Repository;
namespace YumBlazor.Models
{
    public class Category : IEntity 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        

        [Required(ErrorMessage = "Please enter name ...")] 
            public string Name { get; set; } = string.Empty;    
    }
}
