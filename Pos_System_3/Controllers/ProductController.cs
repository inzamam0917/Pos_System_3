using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos_System_3.Services;
using System.Threading.Tasks;
using Pos_System_3.Model;
using Pos_System_3.ApiModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pos_System_3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly string _jwtSubject;

        public ProductController(IProductService productService, IConfiguration configuration)
        {
            _productService = productService;
            _jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT key must be provided.");
            _jwtIssuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer", "JWT issuer must be provided.");
            _jwtAudience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience", "JWT audience must be provided.");
            _jwtSubject = configuration["Jwt:Subject"] ?? throw new ArgumentNullException("Jwt:Subject", "JWT subject must be provided.");
        }


        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO productmodel)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await GetUserFromTokenAsync(token);

            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            try
            {
                Product product = new Product(productmodel.name, productmodel.price, productmodel.quantity, productmodel.type, productmodel.categoryId);

                bool result = await _productService.AddProductAsync(product, token);
                if (result)
                {
                    return Ok(new { message = "Product added successfully." });
                }
                return Unauthorized(new { message = "Not authorized to add product." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO productmodel)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await GetUserFromTokenAsync(token);
            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            Product product = new Product(productmodel.productid, productmodel.name, productmodel.price, productmodel.quantity, productmodel.type, productmodel.categoryId);

            var success = await _productService.UpdateProductAsync(product, token);
            if (!success)
            {
                return BadRequest(new { message = "Failed to update product." });
            }
            return Ok(new { message = "Product updated successfully" });
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await GetUserFromTokenAsync(token);
            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var success = await _productService.DeleteProductAsync(id, token);
            if (!success)
            {
                return BadRequest(new { message = "Failed to delete product." });
            }
            return Ok(new { message = "Product deleted successfully" });
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public async Task<IActionResult> GetProduct(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await GetUserFromTokenAsync(token);
            if (admin == null || admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }
            return Ok(product);
        }

        [HttpGet("getall")]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var admin = await GetUserFromTokenAsync(token);
            if (admin.UserRole != UserRole.Admin)
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }


        [HttpPut("updatequantity")]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityDTO model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await GetUserFromTokenAsync(token);
            if (user == null || (user.UserRole != UserRole.Admin && user.UserRole != UserRole.Cashier))
            {
                return Unauthorized(new { message = "You are not authorized to perform this action." });
            }

            var success = await _productService.UpdateProductQuantityAsync(model.ProductId, model.Quantity, token);
            if (!success)
            {
                return NotFound(new { message = "Product not found or you are not authorized to update the quantity." });
            }
            return Ok(new { message = "Product quantity updated successfully" });
        }

        public async Task<User> GetUserFromTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);

            try
            {
                // Validate token
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtIssuer,
                    ValidAudience = _jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // Extract claims
                var username = principal.FindFirst(ClaimTypes.Name)?.Value;
                var role = principal.FindFirst(ClaimTypes.Role)?.Value;
                var userId = principal.FindFirst("id")?.Value;

                if (username != null && role != null && userId != null)
                {
                    return new User
                    {
                        Username = username,
                        UserRole = Enum.Parse<UserRole>(role),
                        UserID = int.Parse(userId)
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
