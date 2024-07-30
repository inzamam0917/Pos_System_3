using Microsoft.Extensions.Logging;
using Pos_System_3.Repositories;
using Pos_System_3.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Pos_System_3.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private Purchase _currentPurchase;
        private readonly ILogger<PurchaseService> _logger;

        public PurchaseService(IPurchaseRepository purchaseRepository, ILogger<PurchaseService> logger)
        {
            _purchaseRepository = purchaseRepository;
            _logger = logger;
        }

        private async Task<User> GetUserFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = int.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
            return await _purchaseRepository.GetUserByIdAsync(userId);
        }

        public async Task<bool> StartNewPurchaseAsync(string token)
        {
            var user = await GetUserFromTokenAsync(token);
            if (user == null)
            {
                _logger.LogWarning("Invalid user.");
                return false;
            }

            _currentPurchase = new Purchase
            {
                // Initialize other properties if needed
            };

            await _purchaseRepository.AddPurchaseAsync(_currentPurchase);
            await _purchaseRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddProductToPurchaseAsync(int productId, int quantity, string token)
        {
            var user = await GetUserFromTokenAsync(token);
            if (_currentPurchase == null)
            {
                if (!await StartNewPurchaseAsync(token))
                {
                    return false;
                }
            }

            var product = await _purchaseRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product not found.");
                return false;
            }

            var productItem = _currentPurchase.Products.FirstOrDefault(pi => pi.ProductId == productId);
            if (productItem == null)
            {
                _currentPurchase.Products.Add(new ProductItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    PurchaseId = _currentPurchase.PurchaseId
                });
            }
            else
            {
                productItem.Quantity += quantity;
            }

            try
            {
                await _purchaseRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding product to purchase.");
                return false;
            }
        }

        public async Task<bool> RemoveProductFromPurchaseAsync(int productId, int quantity, string token)
        {
            var user = await GetUserFromTokenAsync(token);
            if (_currentPurchase == null)
            {
                _logger.LogWarning("No purchase in progress.");
                return false;
            }

            var productItem = _currentPurchase.Products.FirstOrDefault(pi => pi.ProductId == productId);
            if (productItem == null || productItem.Quantity < quantity)
            {
                _logger.LogWarning("Product not found in purchase or insufficient quantity.");
                return false;
            }

            productItem.Quantity -= quantity;
            if (productItem.Quantity == 0)
            {
                _currentPurchase.Products.Remove(productItem);
            }

            await _purchaseRepository.UpdatePurchaseAsync(_currentPurchase);
            await _purchaseRepository.SaveChangesAsync();

            return true;
        }

        public Purchase GetCurrentPurchase()
        {
            return _currentPurchase;
        }
    }
}
