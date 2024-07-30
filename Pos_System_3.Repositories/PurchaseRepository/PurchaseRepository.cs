using Microsoft.EntityFrameworkCore;
using Pos_System_3.Data;
using Pos_System_3.Model;
using Pos_System_3.Repositories;
using System.Threading.Tasks;

namespace Pos_System_3.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly DBContextEntity _context;

        public PurchaseRepository(DBContextEntity context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task AddPurchaseAsync(Purchase purchase)
        {
            await _context.Purchases.AddAsync(purchase);
        }

        public async Task UpdatePurchaseAsync(Purchase purchase)
        {
            _context.Purchases.Update(purchase);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
