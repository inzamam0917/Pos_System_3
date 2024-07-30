using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_System_3.Model
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();

        public Purchase()
        {
            PurchaseDate = DateTime.Now;
        }

        public Purchase(DateTime purchaseDate)
        {
            PurchaseDate = purchaseDate;
        }


    }
}
