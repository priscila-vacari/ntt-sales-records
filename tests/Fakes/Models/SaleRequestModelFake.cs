using Bogus;
using Sales.API.Models;

namespace Sales.Tests.Fakes.Entities
{
    public class SaleRequestModelFake : Faker<SaleRequestModel>
    {
        public SaleRequestModelFake()
        {
            RuleFor(s => s.Number, f => f.Random.Guid().ToString());
            RuleFor(s => s.Date, f => DateTime.Today);
            RuleFor(s => s.Customer, f => f.Person.FullName);
            RuleFor(s => s.Branch, f => f.Company.CompanyName());
            RuleFor(s => s.Items, f => new SaleItemRequestModelFake().Generate(3));
        }
    }
}
