using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnackShare.Api.Data.Entities.Sales
{
    public class Sale
    {
        public int Id { get; set; }

        public DateTime SaleDate { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
