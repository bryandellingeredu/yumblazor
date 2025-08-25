namespace YumBlazor.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Models.Category>> GetAllCategoriesAsync();
          
    }
}
