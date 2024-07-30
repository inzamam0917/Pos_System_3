using System.Collections.Generic;
using System.Threading.Tasks;
using Pos_System_3.Model;

namespace Pos_System_3.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task AddCategoryAsync(Category category);
        Task<Category> GetCategoryAsync(int id);
        Task<List<Category>> GetAllCategoriesAsync();
        Task UpdateCategoryAsync(Category category);
        Task RemoveCategoryAsync(Category category);
    }
}
