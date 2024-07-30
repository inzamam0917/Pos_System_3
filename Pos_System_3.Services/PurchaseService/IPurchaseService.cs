using Pos_System_3.Model;
using System.Threading.Tasks;

namespace Pos_System_3.Services
{
    public interface IPurchaseService
    {
        Task<bool> StartNewPurchaseAsync(string token);
        Task<bool> AddProductToPurchaseAsync(int productId, int quantity, string token);
        Task<bool> RemoveProductFromPurchaseAsync(int productId, int quantity, string token);
        Purchase GetCurrentPurchase();
    }
}
