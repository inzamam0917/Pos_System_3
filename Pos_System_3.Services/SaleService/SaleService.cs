using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pos_System_3.Model;
using Pos_System_3.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Pos_System_3.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private Sale _currentSale;
        private readonly ILogger<SaleService> _logger;

        public SaleService(ISaleRepository saleRepository, ILogger<SaleService> logger)
        {
            _saleRepository = saleRepository;
            _logger = logger;
        }

        private async Task<User> GetUserFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id");

            if (userIdClaim == null)
            {
                _logger.LogError("User ID claim not found in token.");
                throw new Exception("User ID claim not found in token.");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = await _saleRepository.GetUserByIdAsync(userId);

            if (user != null)
            {
                _logger.LogInformation($"User Details: Id={user.UserID}, Name={user.Name}, Role={user.UserRole}");
            }
            else
            {
                _logger.LogError($"User with ID {userId} not found in the database.");
            }

            return user;
        }

        public async Task<bool> IsCashierAsync(string token)
        {
            var user = await GetUserFromTokenAsync(token);
            if (user != null && user.UserRole == UserRole.Cashier)
            {
                _logger.LogInformation($"User {user.Name} is a cashier.");
                return true;
            }
            _logger.LogWarning($"User is not a cashier or not found. Role: {user?.UserRole}");
            return false;
        }

        public async Task<bool> StartNewSaleAsync(string token)
        {
            var cashier = await GetUserFromTokenAsync(token);
            if (cashier == null || cashier.UserRole != UserRole.Cashier)
            {
                _logger.LogWarning("Invalid cashier.");
                return false;
            }

            _currentSale = new Sale
            {
                Cashier = cashier,
                CashierId = cashier.UserID,
                Date = DateTime.Now,
                Status = SaleStatus.Start
            };

            await _saleRepository.AddSaleAsync(_currentSale);
            await _saleRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddProductToSaleAsync(int productId, int quantity, string token)
        {
            var cashier = await GetUserFromTokenAsync(token);
            if (cashier == null || cashier.UserRole != UserRole.Cashier)
            {
                _logger.LogWarning("Invalid cashier.");
                return false;
            }

            if (_currentSale == null)
            {
                _logger.LogWarning("No sale in progress.");
                return false;
            }

            var product = await _saleRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product not found.");
                return false;
            }

            if (product.Quantity < quantity)
            {
                _logger.LogWarning("Insufficient quantity in inventory.");
                return false;
            }

            _currentSale.AddProduct(product, quantity);
            product.Quantity -= quantity;

            var productItem = _currentSale.Products.FirstOrDefault(pi => pi.ProductId == productId);
            if (productItem != null)
            {
                productItem.Quantity = quantity; // Update quantity if product already in sale
            }
            else
            {
                _currentSale.Products.Add(new ProductItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    SaleId = _currentSale.SaleId
                });
            }

            try
            {
                await _saleRepository.UpdateProductAsync(product);
                await _saleRepository.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency exception occurred while adding product to sale.");
                return false;
            }
        }

        public async Task<bool> RemoveProductFromSaleAsync(int productId, int quantity, string token)
        {
            var cashier = await GetUserFromTokenAsync(token);
            if (_currentSale == null || _currentSale.Cashier != cashier)
            {
                _logger.LogWarning("No sale in progress or wrong cashier.");
                return false;
            }

            var productItem = _currentSale.Products.FirstOrDefault(pi => pi.ProductId == productId);
            if (productItem == null || productItem.Quantity < quantity)
            {
                _logger.LogWarning("Product not found in sale or insufficient quantity.");
                return false;
            }

            productItem.Quantity -= quantity;

            if (productItem.Quantity == 0)
            {
                _currentSale.Products.Remove(productItem);
            }

            var product = await _saleRepository.GetProductByIdAsync(productId);
            if (product != null)
            {
                product.Quantity += quantity;
                await _saleRepository.UpdateProductAsync(product);
            }

            _currentSale.Products.Remove(productItem);
            await _saleRepository.UpdateSaleAsync(_currentSale);
            await _saleRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CompleteSaleAsync(string token)
        {
            var cashier = await GetUserFromTokenAsync(token);
            if (_currentSale != null && _currentSale.Cashier == cashier)
            {
                _currentSale.Date = DateTime.Now;
                _currentSale.Status = SaleStatus.Complete;

                await _saleRepository.UpdateSaleAsync(_currentSale);
                await _saleRepository.SaveChangesAsync();

                _currentSale = null;
                return true;
            }
            return false;
        }

        public Sale GetCurrentSale()
        {
            return _currentSale;
        }
    }
}
