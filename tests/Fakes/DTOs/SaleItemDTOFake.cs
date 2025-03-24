using Bogus;
using Sales.Application.DTOs;

namespace Sales.Tests.Fakes.DTO
{
    public class SaleItemDTOFake : Faker<SaleItemDTO>
    {
        public SaleItemDTOFake()
        {
            RuleFor(s => s.SaleId, f => f.Random.Int(1, 1000));
            RuleFor(p => p.ProductId, f => f.Random.Int(1000, 9999));
            RuleFor(p => p.Quantity, f => f.Random.Int(1, 100));
            RuleFor(p => p.UnitPrice, f => f.Random.Decimal(10, 1000));
            RuleFor(p => p.Discount, f => f.Random.Decimal(10, 1000));
            RuleFor(p => p.TotalValue, f => f.Random.Decimal(10, 1000));
        }
    }
}
