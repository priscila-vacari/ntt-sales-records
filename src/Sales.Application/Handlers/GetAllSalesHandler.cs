using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sales.Application.DTOs;
using Sales.Application.Queries;
using Sales.Domain.Exceptions;
using Sales.Infra.Interfaces;

namespace Sales.Application.Handlers
{
    public class GetAllSalesHandler(ILogger<GetAllSalesHandler> logger, IMapper mapper, IServiceProvider serviceProvider) : IRequestHandler<GetAllSalesQuery, IEnumerable<SaleDTO>>
    {
        private readonly ILogger<GetAllSalesHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<IEnumerable<SaleDTO>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Obtendo todas as vendas.");

            using var scope = _serviceProvider.CreateScope();
            var _saleRepository = scope.ServiceProvider.GetRequiredService<ISaleRepository>();
            var sales = await _saleRepository.GetSalesAsync(request.Page, request.PageSize, request.OrderBy, request.Number, request.Customer, request.Branch, request.IsCancelled, request.TotalValueMin, request.TotalValueMax, i => i.Items);

            if (!sales.Any())
                throw new NotFoundException("Vendas não encontradas.");

            var response = _mapper.Map<IEnumerable<SaleDTO>>(sales);
            return response;
        }
    }
}
