using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sales.Application.Commands;
using Sales.Application.Events;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Domain.Exceptions;
using Sales.Infra.Interfaces;

namespace Sales.Application.Handlers
{
    public class UpdateSaleHandler(ILogger<UpdateSaleHandler> logger, IMapper mapper, IDiscountFactory discountFactory, IServiceProvider serviceProvider) : IRequestHandler<UpdateSaleCommand>
    {
        private readonly ILogger<UpdateSaleHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IDiscountFactory _discountFactory = discountFactory;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;

            _logger.LogInformation("Atualizando a venda de id {id}.", id);

            using var scope = _serviceProvider.CreateScope();
            var _saleRepository = scope.ServiceProvider.GetRequiredService<IRepository<Sale>>();
            var sale = await _saleRepository.GetByIdAsyncIncludes(id, i => i.Items) ?? throw new NotFoundException("Venda não encontrada para o id.");

            var _saleItemRepository = scope.ServiceProvider.GetRequiredService<IRepository<SaleItem>>();
            await _saleItemRepository.DeleteRangeAsync(sale.Items);

            sale = _mapper.Map(request.Sale, sale);
            sale.Id = id;

            foreach (var item in sale.Items)
            {
                var calculateStrategy = _discountFactory.CreateStrategy(item.Quantity);
                calculateStrategy.ApplyDiscount(item);
                item.SaleId = sale.Id;
            }

            sale.TotalValue = sale.Items.Sum(i => i.TotalValue);

            await _saleRepository.UpdateAsync(sale);

            _logger.LogInformation($"await _bus.Publish(new SaleModifiedEvent({sale.Id}));");
        }
    }
}
