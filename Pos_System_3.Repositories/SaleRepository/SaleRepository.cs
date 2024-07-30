using Microsoft.EntityFrameworkCore;
using Pos_System_3.Data;
using Pos_System_3.Model;
using Pos_System_3.Repositories;
using System.Threading.Tasks;

namespace Pos_System_3.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DBContextEntity _context;

        public SaleRepository(DBContextEntity context)
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

        public async Task AddSaleAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            _context.Sales.Update(sale);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
