using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sales.Application.DTOs;
using Sales.Application.Handlers;
using Sales.Application.Queries;
using Sales.Domain.Entities;
using Sales.Domain.Exceptions;
using Sales.Infra.Interfaces;
using Sales.Tests.Fakes.DTO;
using System.Linq.Expressions;

namespace Sales.Tests.Application.Handlers
{
    public class GetSaleByIdHandlerTests
    {
        private readonly ILogger<GetSaleByIdHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Sale> _saleRepository;

        public GetSaleByIdHandlerTests()
        {
            _logger = Substitute.For<ILogger<GetSaleByIdHandler>>();
            _mapper = Substitute.For<IMapper>();
            _serviceProvider = Substitute.For<IServiceProvider>();
            _saleRepository = Substitute.For<IRepository<Sale>>();

            var serviceScope = Substitute.For<IServiceScope>();
            serviceScope.ServiceProvider.Returns(_serviceProvider);

            var scopeFactory = Substitute.For<IServiceScopeFactory>();
            scopeFactory.CreateScope().Returns(serviceScope);

            _serviceProvider.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);
            _serviceProvider.GetService(typeof(IRepository<Sale>)).Returns(_saleRepository);
        }

        [Fact]
        public async Task Should_Return_Sale_When_Found()
        {
            var saleId = 1;
            var saleRequest = new GetSaleByIdQuery(saleId);

            var saleDto = new SaleDTOFake().Generate();

            var sale = new Sale
            {
                Number = saleDto.Number,
                Date = saleDto.Date,
                Customer = saleDto.Customer,
                Branch = saleDto.Branch,
                Items = [.. saleDto.Items.Select(i => new SaleItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })]
            };

            _saleRepository.GetByIdAsyncIncludes(Arg.Any<int>(), Arg.Any<Expression<Func<Sale, object>>[]>()).Returns(Task.FromResult(sale));

            _mapper.Map<SaleDTO>(sale).Returns(saleDto);

            var handler = new GetSaleByIdHandler(_logger, _mapper, _saleRepository);

            var result = await handler.Handle(saleRequest, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(saleDto.Id, result.Id);
            Assert.Equal(saleDto.Number, result.Number);
            Assert.Equal(saleDto.Customer, result.Customer);
            Assert.Equal(saleDto.Branch, result.Branch);

            await _saleRepository.Received(1).GetByIdAsyncIncludes(Arg.Any<int>(), Arg.Any<Expression<Func<Sale, object>>[]>());
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_Sale_Not_Found()
        {
            int invalidId = 999;

            Sale? sale = null;
            _saleRepository.GetByIdAsyncIncludes(Arg.Any<int>(), Arg.Any<Expression<Func<Sale, object>>[]>()).Returns(Task.FromResult(sale));

            var handler = new GetSaleByIdHandler(_logger, _mapper, _saleRepository);
            var query = new GetSaleByIdQuery(invalidId);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
            Assert.Equal("Venda não encontrada para o id.", exception.Message);

            await _saleRepository.Received(1).GetByIdAsyncIncludes(Arg.Any<int>(), Arg.Any<Expression<Func<Sale, object>>[]>());
        }
    }
}