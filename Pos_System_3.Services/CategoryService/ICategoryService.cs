using System.Collections.Generic;
using System.Threading.Tasks;
using Pos_System_3.Model;

namespace Pos_System_3.Services
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(int id, string name);
        Task<bool> RemoveCategoryAsync(int id);
        Task<Category> GetCategoryAsync(int id);
        Task<List<Category>> GetAllCategoriesAsync();
    }
}
