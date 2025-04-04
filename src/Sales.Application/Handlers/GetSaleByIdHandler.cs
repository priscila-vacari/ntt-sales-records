﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sales.Application.DTOs;
using Sales.Application.Queries;
using Sales.Domain.Entities;
using Sales.Domain.Exceptions;
using Sales.Infra.Interfaces;

namespace Sales.Application.Handlers
{
    public class GetSaleByIdHandler(ILogger<GetSaleByIdHandler> logger, IMapper mapper, IRepository<Sale> saleRepository) : IRequestHandler<GetSaleByIdQuery, SaleDTO>
    {
        private readonly ILogger<GetSaleByIdHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IRepository<Sale> _saleRepository = saleRepository;

        public async Task<SaleDTO> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var id = request.Id;    

            _logger.LogInformation("Obtendo a venda de id {id}.", id);

            var sale = await _saleRepository.GetByIdAsyncIncludes(id, i => i.Items) ?? throw new NotFoundException("Venda não encontrada para o id.");

            var response = _mapper.Map<SaleDTO>(sale);
            return response;
        }
    }
}
