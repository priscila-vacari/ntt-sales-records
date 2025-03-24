using Bogus;
using Sales.API.Models;

namespace Sales.Tests.Fakes.Entities
{
    public class SaleItemRequestModelFake : Faker<SaleItemRequestModel>
    {
        public SaleItemRequestModelFake()
        {
            RuleFor(p => p.ProductId, f => f.Random.Int(1000, 9999));
            RuleFor(p => p.Quantity, f => f.Random.Int(1, 100));
            RuleFor(p => p.UnitPrice, f => f.Random.Decimal(10, 1000));
        }
    }
}
