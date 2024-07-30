using System.Collections.Generic;
using System.Threading.Tasks;
using Pos_System_3.Model;
using Pos_System_3.Repositories.CategoryRepository;
using Pos_System_3.Services;

namespace Pos_System_3.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepository.AddCategoryAsync(category);
        }

        public async Task<bool> UpdateCategoryAsync(int id, string name)
        {
            var category = await _categoryRepository.GetCategoryAsync(id);
            if (category == null)
                return false;

            category.Name = name;
            await _categoryRepository.UpdateCategoryAsync(category);
            return true;
        }

        public async Task<bool> RemoveCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryAsync(id);
            if (category == null)
                return false;

            await _categoryRepository.RemoveCategoryAsync(category);
            return true;
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _categoryRepository.GetCategoryAsync(id);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }
    }
}
