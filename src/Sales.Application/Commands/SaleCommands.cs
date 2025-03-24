using MediatR;
using Sales.Application.DTOs;

namespace Sales.Application.Commands
{
    public record CreateSaleCommand(SaleDTO Sale) : IRequest<SaleDTO>;
    public record UpdateSaleCommand(int Id, SaleDTO Sale) : IRequest;
    public record DeleteSaleCommand(int Id) : IRequest;
}
