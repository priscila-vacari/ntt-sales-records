using FluentValidation;
using Sales.API.Models;

namespace Sales.API.Validators
{
    /// <summary>
    /// Classe de validação do item da venda
    /// </summary>
    public class SaleItemRequestModelValidator : AbstractValidator<SaleItemRequestModel>
    {
        /// <summary>
        /// Validador de item da venda
        /// </summary>
        public SaleItemRequestModelValidator()
        {
            RuleFor(item => item.ProductId)
                .GreaterThan(0)
                .WithMessage("O ProductId deve ser maior que zero.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0)
                .WithMessage("A Quantidade deve ser maior que zero.");

            RuleFor(x => x.UnitPrice)
               .GreaterThan(0).WithMessage("O Valor deve ser maior que zero.")
               .PrecisionScale(10, 2, true).WithMessage("O Valor deve ter no máximo 10 dígitos e 2 casas decimais.");
        }
    }
}
