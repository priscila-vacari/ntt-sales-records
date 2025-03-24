using FluentValidation;
using Sales.API.Models;

namespace Sales.API.Validators
{
    /// <summary>
    /// Classe de validação de venda
    /// </summary>
    public class SaleRequestModelValidator : AbstractValidator<SaleRequestModel>
    {
        /// <summary>
        /// Validador de venda
        /// </summary>
        public SaleRequestModelValidator()
        {
            RuleFor(item => item.Number)
                .NotEmpty()
                .WithMessage("O Number deve ser preenchido.");

            RuleFor(item => item.Date)
                .NotEmpty().WithMessage("A data é obrigatória.")
                .GreaterThanOrEqualTo(DateTime.Now.AddMonths(-12)).WithMessage("A data deve ser de 1 ano atrás para frente.")
                .LessThan(DateTime.Now).WithMessage("A data deve ser no máximo igual a hoje.");

            RuleFor(item => item.Customer)
                .NotEmpty()
                .WithMessage("O Customer deve ser preenchido.");

            RuleFor(item => item.Branch)
                .NotEmpty()
                .WithMessage("O Branch deve ser preenchido.");

            RuleFor(order => order.Items)
                .NotNull()
                .WithMessage("A lista de itens não pode ser nula.")
                .NotEmpty()
                .WithMessage("A lista de itens deve conter pelo menos um item.");

            RuleForEach(order => order.Items)
                .SetValidator(new SaleItemRequestModelValidator());
        }
    }
}
