using Sales.Application.Interfaces;
using Sales.Application.Strategies;

namespace Sales.Application.Factories
{
    public class DiscountFactory() : IDiscountFactory
    {
        public IDiscountStrategy CreateStrategy(int quantity)
        {
            return quantity switch
            {
                > 20 => throw new InvalidOperationException("Não é possível vender mais de 20 itens idênticos."),
                >= 10 => new DiscountStrategy(20),
                > 4 => new DiscountStrategy(10),
                _ => new DiscountStrategy(0),
            };
        }
    }
}
