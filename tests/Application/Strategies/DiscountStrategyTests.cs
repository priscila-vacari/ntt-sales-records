using Bogus;
using Sales.Application.Strategies;
using Sales.Domain.Entities;

namespace Sales.Tests.Application.Strategies
{
    public class DiscountStrategyTests
    {

        [Fact]
        public void Should_Return_Correct_Discount_Percentage()
        {
            var discountValue = 10m; 
            var discountStrategy = new DiscountStrategy(discountValue);

            var result = discountStrategy.DiscountPercentage();

            Assert.Equal(discountValue, result);
        }

        [Fact]
        public void Should_Apply_Discount_To_SaleItem()
        {
            var discountValue = 10m; 
            var saleItem = new Faker<SaleItem>()
                .RuleFor(s => s.UnitPrice, f => f.Finance.Amount(1, 100))
                .RuleFor(s => s.Quantity, f => f.Random.Int(1, 10))
                .Generate();

            var discountStrategy = new DiscountStrategy(discountValue);

            discountStrategy.ApplyDiscount(saleItem);

            Assert.Equal(discountValue, saleItem.Discount); 
            var expectedTotalValue = (saleItem.UnitPrice * saleItem.Quantity) * (1 - discountValue / 100);
            Assert.Equal(expectedTotalValue, saleItem.TotalValue);
        }

        [Fact]
        public void Should_Calculate_Correct_TotalValue_When_Discount_Applied()
        {
            var discountValue = 15m; 
            var saleItem = new SaleItem
            {
                UnitPrice = 50m,
                Quantity = 2 
            };
            var discountStrategy = new DiscountStrategy(discountValue);

            discountStrategy.ApplyDiscount(saleItem);

            var expectedTotalValue = (saleItem.UnitPrice * saleItem.Quantity) * (1 - discountValue / 100);
            Assert.Equal(expectedTotalValue, saleItem.TotalValue);
        }

        [Fact]
        public void Should_Not_Apply_Discount_If_Zero_Discount_Value()
        {
            var discountValue = 0m;
            var saleItem = new Faker<SaleItem>()
                .RuleFor(s => s.UnitPrice, f => f.Finance.Amount(1, 100))
                .RuleFor(s => s.Quantity, f => f.Random.Int(1, 10))
                .Generate();

            var discountStrategy = new DiscountStrategy(discountValue);

            discountStrategy.ApplyDiscount(saleItem);

            Assert.Equal(discountValue, saleItem.Discount); 
            var expectedTotalValue = saleItem.UnitPrice * saleItem.Quantity;
            Assert.Equal(expectedTotalValue, saleItem.TotalValue); 
        }
    }
}
