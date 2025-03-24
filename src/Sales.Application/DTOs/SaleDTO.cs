namespace Sales.Application.DTOs
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Customer { get; set; } = string.Empty;
        public decimal TotalValue { get; set; }
        public string Branch { get; set; } = string.Empty;
        public bool IsCancelled { get; set; }
        public virtual List<SaleItemDTO> Items { get; set; } = [];
    }
}
