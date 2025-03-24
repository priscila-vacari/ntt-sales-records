using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Domain.Entities
{
    [Table("sale_items")]
    public class SaleItem
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalValue { get; set; }

        public virtual Sale Sale { get; set; } = new Sale();
    }
}
