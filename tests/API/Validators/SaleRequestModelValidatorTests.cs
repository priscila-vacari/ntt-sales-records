using Sales.API.Models;
using Sales.API.Validators;
using Bogus;
using FluentValidation.TestHelper;

namespace Sales.Tests.API.Validators
{
    public class SaleRequestModelValidatorTests
    {
        private readonly SaleRequestModelValidator _validator;
        private readonly Faker<SaleRequestModel> _saleRequestModelFaker;
        
        public SaleRequestModelValidatorTests()
        {
            _validator = new SaleRequestModelValidator();

            _saleRequestModelFaker = new Faker<SaleRequestModel>()
                .RuleFor(x => x.Number, f => f.Commerce.Ean13())
                .RuleFor(x => x.Date, f => f.Date.Past(1))
                .RuleFor(x => x.Customer, f => f.Name.FullName())
                .RuleFor(x => x.Branch, f => f.Company.CompanyName())
                .RuleFor(x => x.Items, f => [ new SaleItemRequestModel { ProductId = f.Random.Int(1, 100), Quantity = f.Random.Int(1, 10), UnitPrice = f.Finance.Amount() } ]);
        }

        [Fact]
        public void ShouldHaveErrorWhenNumberIsEmpty()
        {
            var saleRequest = _saleRequestModelFaker.Generate();
            saleRequest.Number = string.Empty;

            var result = _validator.TestValidate(saleRequest);
            result.ShouldHaveValidationErrorFor(x => x.Number)
                .WithErrorMessage("O Number deve ser preenchido.");
        }

        [Fact]
        public void ShouldHaveErrorWhenDateIsEmpty()
        {
            var saleRequest = _saleRequestModelFaker.Generate();
            saleRequest.Date = default;

            var result = _validator.TestValidate(saleRequest);
            result.ShouldHaveValidationErrorFor(x => x.Date)
                .WithErrorMessage("A data é obrigatória.");
        }

        [Fact]
        public void ShouldHaveErrorWhenDateIsLessThanOneYearAgo()
        {
            var saleRequest = _saleRequestModelFaker.Generate();
            saleRequest.Date = DateTime.Now.AddYears(-2); // Data há mais de 1 ano

            var result = _validator.TestValidate(saleRequest);
            result.ShouldHaveValidationErrorFor(x => x.Date)
                .WithErrorMessage("A data deve ser de 1 ano atrás para frente.");
        }

        [Fact]
        public void ShouldHaveErrorWhenDateIsGreaterThanToday()
        {
            var saleRequest = _saleRequestModelFaker.Generate();
            saleRequest.Date = DateTime.Now.AddDays(1); // Data no futuro

            var result = _validator.TestValidate(saleRequest);
            result.ShouldHaveValidationErrorFor(x => x.Date)
                .WithErrorMessage("A data deve ser no máximo igual a hoje.");
        }

        [Fact]
        public void ShouldHaveErrorWhenCustomerIsEmpty()
        {
            var saleRequest = _saleRequestModelFaker.Generate();
            saleRequest.Customer = string.Empty;

            var result = _validator.TestValidate(saleRequest);
            result.ShouldHaveValidationErrorFor(x => x.Customer)
                .WithErrorMessage("O Customer deve ser preenchido.");
        }

        [Fact]
        public void ShouldHaveErrorWhenBranchIsEmpty()
        {
            var saleRequest = _saleRequestModelFaker.Generate();
            saleRequest.Branch = string.Empty;

            var result = _validator.TestValidate(saleRequest);
            result.ShouldHaveValidationErrorFor(x => x.Branch)
                .WithErrorMessage("O Branch deve ser preenchido.");
        }

        [Fact]
        public void ShouldHaveErrorWhenItemsIsNull()
        {
            var saleRequest = _saleRequestModelFaker.Generate();
            saleRequest.Items = null;

            var result = _validator.TestValidate(saleRequest);
            result.ShouldHaveValidationErrorFor(x => x.Items)
                .WithErrorMessage("A lista de itens não pode ser nula.");
        }

        [Fact]
        public void ShouldHaveErrorWhenItemsIsEmpty()
        {
            var saleRequest = _saleRequestModelFaker.Generate();
            saleRequest.Items = [];

            var result = _validator.TestValidate(saleRequest);
            result.ShouldHaveValidationErrorFor(x => x.Items)
                .WithErrorMessage("A lista de itens deve conter pelo menos um item.");
        }

        [Fact]
        public void ShouldValidateSaleRequestModelCorrectlyWhenValid()
        {
            var saleRequest = _saleRequestModelFaker.Generate();

            var result = _validator.TestValidate(saleRequest);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
