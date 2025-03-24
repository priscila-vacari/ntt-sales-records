using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sales.Application.Commands;
using Sales.Application.Events;
using Sales.Domain.Entities;
using Sales.Domain.Exceptions;
using Sales.Infra.Interfaces;

namespace Sales.Application.Handlers
{
    public class DeleteSaleHandler(ILogger<DeleteSaleHandler> logger, IServiceProvider serviceProvider) : IRequestHandler<DeleteSaleCommand>
    {
        private readonly ILogger<DeleteSaleHandler> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;

            _logger.LogInformation("Cancelando a venda de id {id}.", id);

            using var scope = _serviceProvider.CreateScope();
            var _saleRepository = scope.ServiceProvider.GetRequiredService<IRepository<Sale>>();
            var sale = await _saleRepository.GetByIdAsync(id) ?? throw new NotFoundException("Venda não encontrada para o id.");

            sale.IsCancelled = true;

            await _saleRepository.UpdateAsync(sale);

            foreach (var item in sale.Items)
                logger.LogInformation($"await _bus.Publish(new SaleItemCancelledEvent({item.SaleId}, {item.ProductId}));");

            _logger.LogInformation($"await _bus.Publish(new SaleCancelledEvent({sale.Id}));");
        }
    }
}
