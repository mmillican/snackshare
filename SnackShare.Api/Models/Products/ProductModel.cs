using System.ComponentModel.DataAnnotations;

namespace SnackShare.Api.Models.Products
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public decimal SalePrice { get; set; }

        public int StockQuantity { get; set; }
    }
}
