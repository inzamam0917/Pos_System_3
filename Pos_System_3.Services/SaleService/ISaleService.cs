using Pos_System_3.Model;
using System.Threading.Tasks;

namespace Pos_System_3.Services
{
    public interface ISaleService
    {
        Task<bool> IsCashierAsync(string token);
        Task<bool> StartNewSaleAsync(string token);
        Task<bool> AddProductToSaleAsync(int productId, int quantity, string token);
        Task<bool> RemoveProductFromSaleAsync(int productId, int quantity, string token);
        Task<bool> CompleteSaleAsync(string token);
        Sale GetCurrentSale();
    }
}
