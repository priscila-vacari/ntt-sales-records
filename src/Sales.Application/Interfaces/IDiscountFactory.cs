namespace Sales.Application.Interfaces
{
    public interface IDiscountFactory
    {
        IDiscountStrategy CreateStrategy(int quantity);
    }
}
