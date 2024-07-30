using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_System_3.Model
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Type length can't be more than 50.")]
        public string Type { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();

        public Product() { }

        public Product(string name, decimal price, int quantity, string type, int categoryId)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
            CategoryId = categoryId;
        }

        public Product(int productid, string name, decimal price, int quantity, string type, int categoryId)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
            CategoryId = categoryId;
            ProductId = productid;
        }

        public Product(string name, decimal price, int quantity, string type)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
        }


    }
}
