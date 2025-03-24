using Bogus;
using Sales.Application.DTOs;

namespace Sales.Tests.Fakes.DTO
{
    public class SaleDTOFake : Faker<SaleDTO>
    {
        public SaleDTOFake()
        {
            RuleFor(s => s.Id, f => f.Random.Int(1, 1000));
            RuleFor(s => s.Number, f => f.Random.Guid().ToString());
            RuleFor(s => s.Date, f => DateTime.Today);
            RuleFor(s => s.Customer, f => f.Person.FullName);
            RuleFor(s => s.TotalValue, f => f.Finance.Amount(10, 500));
            RuleFor(s => s.Branch, f => f.Company.CompanyName());
            RuleFor(s => s.IsCancelled, f => false);
            RuleFor(s => s.Items, f => new SaleItemDTOFake().Generate(3));
        }
    }
}
