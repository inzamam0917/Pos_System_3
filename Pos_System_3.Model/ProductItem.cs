using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_System_3.Model
{
    public class ProductItem
    {
        [Key]
        public int ProductItemId { get; set; }

        [Required]
        public int SaleId { get; set; }

        public Sale Sale { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        public Purchase Purchase { get; set; }

        [Required]
        public int PurchaseId { get; set; }
        public ProductItem() { }

        public ProductItem(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public ProductItem(Product product, int quantity)
        {
            Product = product;
            ProductId = product.ProductId;
            Quantity = quantity;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public decimal TotalAmount
        {
            get
            {
                return Product.Price * Quantity;
            }
        }
    }
}
