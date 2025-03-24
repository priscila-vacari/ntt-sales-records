using MediatR;
using Sales.Application.DTOs;

namespace Sales.Application.Queries
{
    public record GetSaleByIdQuery(int Id) : IRequest<SaleDTO>;
    public record GetAllSalesQuery(int Page, int PageSize, string[]? OrderBy, string? Number, string? Customer, string? Branch, bool? IsCancelled, decimal? TotalValueMin, decimal? TotalValueMax) : IRequest<IEnumerable<SaleDTO>>;
}
