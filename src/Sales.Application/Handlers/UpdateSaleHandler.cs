using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Sales.Application.Commands;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Domain.Exceptions;
using Sales.Infra.Interfaces;

namespace Sales.Application.Handlers
{
    public class UpdateSaleHandler(ILogger<UpdateSaleHandler> logger, IMapper mapper, IDiscountFactory discountFactory, IRepository<Sale> saleRepository, IRepository<SaleItem> saleItemRepository) : IRequestHandler<UpdateSaleCommand>
    {
        private readonly ILogger<UpdateSaleHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IDiscountFactory _discountFactory = discountFactory;
        private readonly IRepository<Sale> _saleRepository = saleRepository;
        private readonly IRepository<SaleItem> _saleItemRepository = saleItemRepository;

        public async Task Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;

            _logger.LogInformation("Atualizando a venda de id {id}.", id);

            var sale = await _saleRepository.GetByIdAsyncIncludes(id, i => i.Items) ?? throw new NotFoundException("Venda não encontrada para o id.");

            await _saleItemRepository.DeleteRangeAsync(sale.Items);

            sale = _mapper.Map(request.Sale, sale);
            sale.Id = id;

            CalculateDiscount(sale);

            await _saleRepository.UpdateAsync(sale);

            _logger.LogInformation($"await _bus.Publish(new SaleModifiedEvent({sale.Id}));");
        }

        private void CalculateDiscount(Sale sale)
        {
            foreach (var item in sale.Items)
            {
                var calculateStrategy = _discountFactory.CreateStrategy(item.Quantity);
                calculateStrategy.ApplyDiscount(item);
                item.SaleId = sale.Id;
            }

            sale.TotalValue = sale.Items.Sum(i => i.TotalValue);
        }
    }
}
