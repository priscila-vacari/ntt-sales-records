using FluentAssertions;
using Sales.Application.Factories;
using Sales.Application.Strategies;

namespace Sales.Tests.Application.Factory
{
    public class DiscountFactoryTests
    {
        private readonly DiscountFactory _discountFactory;

        public DiscountFactoryTests()
        {
            _discountFactory = new DiscountFactory();
        }

        [Fact]
        public void CreateStrategy_ShouldThrowException_WhenQuantityGreaterThan20()
        {
            int quantity = 21;

            var exception = Assert.Throws<InvalidOperationException>(() => _discountFactory.CreateStrategy(quantity));
            Assert.Equal("Não é possível vender mais de 20 itens idênticos.", exception.Message);
        }

        [Fact]
        public void CreateStrategy_ShouldReturn20PercentDiscount_WhenQuantityIsGreaterThanOrEqualTo10()
        {
            int quantity = 10;

            var discountStrategy = _discountFactory.CreateStrategy(quantity);

            Assert.Equal(20, discountStrategy.DiscountPercentage());
        }

        [Fact]
        public void CreateStrategy_ShouldReturn10PercentDiscount_WhenQuantityIsGreaterThan4()
        {
            int quantity = 5;

            var discountStrategy = _discountFactory.CreateStrategy(quantity);

            Assert.Equal(10, discountStrategy.DiscountPercentage());
        }

        [Fact]
        public void CreateStrategy_ShouldReturn0PercentDiscount_WhenQuantityIsLessThan4()
        {
            int quantity = 3;

            var discountStrategy = _discountFactory.CreateStrategy(quantity);

            Assert.Equal(0, discountStrategy.DiscountPercentage());
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(4, 0)]
        [InlineData(5, 10)]
        [InlineData(10, 20)]
        [InlineData(20, 20)]
        [InlineData(21, 20)] 
        public void CreateStrategy_ShouldReturnCorrectDiscount(int quantity, int expectedDiscount)
        {
            if (quantity > 20)
            {
                var exception = Assert.Throws<InvalidOperationException>(() => _discountFactory.CreateStrategy(quantity));
                Assert.Equal("Não é possível vender mais de 20 itens idênticos.", exception.Message);
            }
            else
            {
                var discountStrategy = _discountFactory.CreateStrategy(quantity);
                Assert.Equal(expectedDiscount, discountStrategy.DiscountPercentage());
            }
        }
    }
}
