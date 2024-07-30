using System.Collections.Generic;
using System.Threading.Tasks;
using Pos_System_3.Model;

namespace Pos_System_3.Services
{
    public interface IProductService
    {
        Task<bool> AddProductAsync(Product product, string token);
        Task<bool> UpdateProductAsync(Product product, string token);
        Task<bool> RemoveProductAsync(int productId, string token);
        Task<bool> DeleteProductAsync(int productId, string token);
        Task<bool> UpdateProductQuantityAsync(int productId, int quantity, string token);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(int productId);
    }
}
