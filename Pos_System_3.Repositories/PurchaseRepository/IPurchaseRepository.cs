using Pos_System_3.Model;
using System.Threading.Tasks;

namespace Pos_System_3.Repositories
{
    public interface IPurchaseRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<Product> GetProductByIdAsync(int productId);
        Task AddPurchaseAsync(Purchase purchase);
        Task UpdatePurchaseAsync(Purchase purchase);
        Task SaveChangesAsync();
    }
}
