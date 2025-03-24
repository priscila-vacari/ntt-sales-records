using Bogus;
using FluentValidation.TestHelper;
using Sales.API.Models;
using Sales.API.Validators;

namespace Sales.Tests.API.Validators
{
    public class SaleItemRequestModelValidatorTests
    {
        private readonly SaleItemRequestModelValidator _validator;
        private readonly Faker<SaleItemRequestModel> _saleItemRequestModelFaker;

        public SaleItemRequestModelValidatorTests()
        {
            _validator = new SaleItemRequestModelValidator();

            _saleItemRequestModelFaker = new Faker<SaleItemRequestModel>()
                .RuleFor(x => x.ProductId, f => f.Random.Int(1, 100))
                .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(p => p.UnitPrice, f => Math.Round(f.Random.Decimal(0.01m, 1000.00m), 2));
        }

        [Fact]
        public void ShouldHaveErrorWhenProductIdIsZero()
        {
            var saleItemRequest = _saleItemRequestModelFaker.Generate();
            saleItemRequest.ProductId = 0;

            var result = _validator.TestValidate(saleItemRequest);
            result.ShouldHaveValidationErrorFor(x => x.ProductId)
                .WithErrorMessage("O ProductId deve ser maior que zero.");
        }

        [Fact]
        public void ShouldHaveErrorWhenQuantityIsZero()
        {
            var saleItemRequest = _saleItemRequestModelFaker.Generate();
            saleItemRequest.Quantity = 0;

            var result = _validator.TestValidate(saleItemRequest);
            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                .WithErrorMessage("A Quantidade deve ser maior que zero.");
        }

        [Fact]
        public void ShouldHaveErrorWhenUnitPriceIsZero()
        {
            var saleItemRequest = _saleItemRequestModelFaker.Generate();
            saleItemRequest.UnitPrice = 0;

            var result = _validator.TestValidate(saleItemRequest);
            result.ShouldHaveValidationErrorFor(x => x.UnitPrice)
                .WithErrorMessage("O Valor deve ser maior que zero.");
        }

        [Fact]
        public void ShouldHaveErrorWhenUnitPriceHasTooManyDecimals()
        {
            var saleItemRequest = _saleItemRequestModelFaker.Generate();
            saleItemRequest.UnitPrice = 123.12345m; // Excedendo as 2 casas decimais

            var result = _validator.TestValidate(saleItemRequest);
            result.ShouldHaveValidationErrorFor(x => x.UnitPrice)
                .WithErrorMessage("O Valor deve ter no máximo 10 dígitos e 2 casas decimais.");
        }

        [Fact]
        public void ShouldValidateSaleItemRequestModelCorrectlyWhenValid()
        {
            var saleItemRequest = _saleItemRequestModelFaker.Generate();

            var result = _validator.TestValidate(saleItemRequest);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
