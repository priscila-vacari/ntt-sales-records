using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sales.Application.Commands;
using Sales.Application.DTOs;
using Sales.Application.Events;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Domain.Exceptions;
using Sales.Infra.Interfaces;

namespace Sales.Application.Handlers
{
    public class CreateSaleHandler(ILogger<CreateSaleHandler> logger, IMapper mapper, IDiscountFactory discountFactory, IServiceProvider serviceProvider) : IRequestHandler<CreateSaleCommand, SaleDTO>
    {
        private readonly ILogger<CreateSaleHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IDiscountFactory _discountFactory = discountFactory;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<SaleDTO> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var saleRequest = request.Sale;

            _logger.LogInformation("Criando nova venda.");
            var sale = _mapper.Map<Sale>(saleRequest);

            using var scope = _serviceProvider.CreateScope();
            var _saleRepository = scope.ServiceProvider.GetRequiredService<IRepository<Sale>>();

            var saleExists = await _saleRepository.GetWhereAsync(o => o.Number == sale.Number && o.Date == sale.Date);
            if (saleExists.Any())
                throw new DuplicateEntryException("Venda já cadastrada");

            foreach (var item in sale.Items)
            {
                var calculateStrategy = _discountFactory.CreateStrategy(item.Quantity);
                calculateStrategy.ApplyDiscount(item);
            }

            sale.TotalValue = sale.Items.Sum(i => i.TotalValue);

            await _saleRepository.AddAsync(sale);
            
            _logger.LogInformation("await _bus.Publish(new SaleCreatedEvent({id}));", sale.Id);

            var response = _mapper.Map<SaleDTO>(sale);
            return response;
        }
    }
}
