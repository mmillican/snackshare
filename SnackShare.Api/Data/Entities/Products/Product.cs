using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnackShare.Api.Data.Entities.Products
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public decimal SalePrice { get; set; }

        public int StockQuantity { get; set; }
        
    }
}
