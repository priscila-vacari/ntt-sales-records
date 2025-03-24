using System.Diagnostics.CodeAnalysis;

namespace Sales.Application.Events
{
    [ExcludeFromCodeCoverage]
    public record SaleCreatedEvent(int SaleId);

    [ExcludeFromCodeCoverage]
    public record SaleModifiedEvent(int SaleId);

    [ExcludeFromCodeCoverage]
    public record SaleCancelledEvent(int SaleId);

    [ExcludeFromCodeCoverage]
    public record SaleItemCancelledEvent(int SaleId, int ProductId);
}
