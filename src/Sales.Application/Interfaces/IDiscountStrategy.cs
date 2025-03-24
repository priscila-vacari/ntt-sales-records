using Sales.Domain.Entities;

namespace Sales.Application.Interfaces
{
    public interface IDiscountStrategy
    {
        void ApplyDiscount(SaleItem item);
        decimal DiscountPercentage();
    }
}
