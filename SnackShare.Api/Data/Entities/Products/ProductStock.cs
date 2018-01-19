using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnackShare.Api.Data.Entities.Products
{
    public class ProductStock
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public DateTime PurchaseDate { get; set; }

        public int PurchaseQuantity { get; set; }
        public decimal PurchasePrice { get; set; }

        public decimal UnitPrice { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
