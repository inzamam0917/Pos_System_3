using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pos_System_3.Model;
using Pos_System_3.Repositories.CategoryRepository;
using Pos_System_3.Repositories.ProductRepository;
using Pos_System_3.Repositories.UserRepository;
using Pos_System_3.Services;

namespace Pos_System_3.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> AddProductAsync(Product product, string token)
        {
            var admin = await GetUserFromTokenAsync(token);
            if (admin != null && admin.UserRole == UserRole.Admin)
            {
                var categoryExists = await _productRepository.CategoryExistsAsync(product.CategoryId);
                if (!categoryExists)
                {
                    throw new Exception("Category not found.");
                }

                product.Category = await _categoryRepository.GetCategoryAsync(product.CategoryId);

                await _productRepository.AddProductAsync(product);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateProductAsync(Product product, string token)
        {
            var admin = await GetUserFromTokenAsync(token);
            if (admin != null && admin.UserRole == UserRole.Admin)
            {
                var existingProduct = await _productRepository.GetProductAsync(product.ProductId);

                if (existingProduct == null)
                {
                    throw new Exception("Product not found.");
                }

                var categoryExists = await _productRepository.CategoryExistsAsync(product.CategoryId);
                if (!categoryExists)
                {
                    throw new Exception("Category not found.");
                }

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                existingProduct.Type = product.Type;
                existingProduct.CategoryId = product.CategoryId;

                await _productRepository.UpdateProductAsync(existingProduct);
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveProductAsync(int productId, string token)
        {
            var admin = await GetUserFromTokenAsync(token);
            if (admin != null && admin.UserRole == UserRole.Admin)
            {
                var product = await _productRepository.GetProductAsync(productId);
                if (product != null)
                {
                    await _productRepository.RemoveProductAsync(product);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteProductAsync(int productId, string token)
        {
            return await RemoveProductAsync(productId, token);
        }

        public async Task<bool> UpdateProductQuantityAsync(int productId, int quantity, string token)
        {
            var admin = await GetUserFromTokenAsync(token);
            if (admin != null && admin.UserRole == UserRole.Admin)
            {
                var product = await _productRepository.GetProductAsync(productId);
                if (product != null)
                {
                    product.Quantity = quantity;
                    await _productRepository.UpdateProductAsync(product);
                    return true;
                }
            }
            return false;
        }

        public async Task<User> GetUserFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = int.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _productRepository.GetProductAsync(productId);
        }
    }
}
