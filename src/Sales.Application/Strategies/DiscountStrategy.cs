using Sales.Application.Interfaces;
using Sales.Domain.Entities;

namespace Sales.Application.Strategies
{
    public class DiscountStrategy(decimal discountValue) : IDiscountStrategy
    {
        private readonly decimal _discountValue = discountValue;
        public decimal DiscountPercentage() => _discountValue;

        public void ApplyDiscount(SaleItem item)
        {
            item.Discount = _discountValue;
            item.TotalValue = Calculate(item.UnitPrice, item.Quantity);
        }

        private decimal Calculate(decimal price, int quantity)
        {
            return (price * quantity) * (1 - _discountValue / 100);
        }
    }
}
