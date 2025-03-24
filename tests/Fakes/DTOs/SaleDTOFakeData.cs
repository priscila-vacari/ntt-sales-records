namespace Sales.Tests.Fakes.DTO
{
    public class SaleDTOFakeData : SaleDTOFake
    {
        public SaleDTOFakeData(int? id = null)
        {
            if (id != null)
                RuleFor(p => p.Id, id);
        }
    }
}
