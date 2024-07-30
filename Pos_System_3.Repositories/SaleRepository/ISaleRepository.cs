using Pos_System_3.Model;
using System.Threading.Tasks;

namespace Pos_System_3.Repositories
{
    public interface ISaleRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<Product> GetProductByIdAsync(int productId);
        Task AddSaleAsync(Sale sale);
        Task UpdateSaleAsync(Sale sale);
        Task UpdateProductAsync(Product product);
        Task SaveChangesAsync();
    }
}
