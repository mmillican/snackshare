using System;

namespace SnackShare.Api.Models.Products
{
    public class ProductStockModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public int PurchaseQuantity { get; set; }
        public decimal PurchasePrice { get; set; }

        public decimal UnitPrice { get; set; }

        public int UserId { get; set; }
    }
}
