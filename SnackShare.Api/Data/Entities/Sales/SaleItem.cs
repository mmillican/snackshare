using System.ComponentModel.DataAnnotations.Schema;
using SnackShare.Api.Data.Entities.Products;

namespace SnackShare.Api.Data.Entities.Sales
{
    public class SaleItem
    {
        public int Id { get; set; }

        public int SaleId { get; set; }
        [ForeignKey(nameof(SaleId))]
        public virtual Sale Sale { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal LineTotal { get; set; }
    }
}
