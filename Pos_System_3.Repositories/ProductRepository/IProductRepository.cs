using System.Collections.Generic;
using System.Threading.Tasks;
using Pos_System_3.Model;

namespace Pos_System_3.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductAsync(int productId);
        Task<List<Product>> GetAllProductsAsync();
        Task UpdateProductAsync(Product product);
        Task RemoveProductAsync(Product product);
        Task<bool> CategoryExistsAsync(int categoryId);
    }
}
