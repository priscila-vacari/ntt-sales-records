using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Sales.Application.DTOs;
using Sales.Application.Queries;
using Sales.Domain.Exceptions;
using Sales.Infra.Interfaces;

namespace Sales.Application.Handlers
{
    public class GetAllSalesHandler(ILogger<GetAllSalesHandler> logger, IMapper mapper, ISaleRepository saleRepository) : IRequestHandler<GetAllSalesQuery, IEnumerable<SaleDTO>>
    {
        private readonly ILogger<GetAllSalesHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly ISaleRepository _saleRepository = saleRepository;

        public async Task<IEnumerable<SaleDTO>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Obtendo todas as vendas.");

            var sales = await _saleRepository.GetSalesAsync(request.Page, request.PageSize, request.OrderBy, request.Number, request.Customer, request.Branch, request.IsCancelled, request.TotalValueMin, request.TotalValueMax, i => i.Items);

            if (!sales.Any())
                throw new NotFoundException("Vendas não encontradas.");

            var response = _mapper.Map<IEnumerable<SaleDTO>>(sales);
            return response;
        }
    }
}
