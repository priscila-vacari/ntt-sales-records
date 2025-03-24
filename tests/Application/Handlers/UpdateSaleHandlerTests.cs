using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sales.Application.Commands;
using Sales.Application.DTOs;
using Sales.Application.Handlers;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Domain.Exceptions;
using Sales.Infra.Interfaces;
using Sales.Tests.Fakes.DTO;
using System.Linq.Expressions;

namespace Sales.Tests.Application.Handlers
{
    public class UpdateSaleHandlerTests
    {
        private readonly ILogger<UpdateSaleHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDiscountFactory _discountFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<SaleItem> _saleItemRepository;

        public UpdateSaleHandlerTests()
        {
            _logger = Substitute.For<ILogger<UpdateSaleHandler>>();
            _mapper = Substitute.For<IMapper>();
            _discountFactory = Substitute.For<IDiscountFactory>();
            _serviceProvider = Substitute.For<IServiceProvider>();
            _saleRepository = Substitute.For<IRepository<Sale>>();
            _saleItemRepository = Substitute.For<IRepository<SaleItem>>();

            var serviceScope = Substitute.For<IServiceScope>();
            serviceScope.ServiceProvider.Returns(_serviceProvider);

            var scopeFactory = Substitute.For<IServiceScopeFactory>();
            scopeFactory.CreateScope().Returns(serviceScope);

            _serviceProvider.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);
            _serviceProvider.GetService(typeof(IRepository<Sale>)).Returns(_saleRepository);
            _serviceProvider.GetService(typeof(IRepository<SaleItem>)).Returns(_saleItemRepository);
        }


        [Fact]
        public async Task Should_Update_Sale_Correctly()
        {
            var saleId = 1;
            var saleRequest = new UpdateSaleCommand(saleId, new SaleDTOFake().Generate());

            var sale = new Sale
            {
                Number = saleRequest.Sale.Number,
                Date = saleRequest.Sale.Date,
                Customer = saleRequest.Sale.Customer,
                Branch = saleRequest.Sale.Branch,
                Items = [.. saleRequest.Sale.Items.Select(i => new SaleItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })]
            };

            _saleRepository.GetByIdAsyncIncludes(Arg.Any<int>(), Arg.Any<Expression<Func<Sale, object>>[]>()).Returns(Task.FromResult(sale));
            _saleItemRepository.DeleteRangeAsync(Arg.Any<List<SaleItem>>()).Returns(Task.CompletedTask);
            _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask);

            var handler = new UpdateSaleHandler(_logger, _mapper, _discountFactory, _serviceProvider);

            _mapper.Map(Arg.Any<SaleDTO>(), Arg.Any<Sale>()).Returns(sale); 

            await handler.Handle(saleRequest, CancellationToken.None);

            await _saleItemRepository.Received(1).DeleteRangeAsync(Arg.Any<List<SaleItem>>()); 
            await _saleRepository.Received(1).UpdateAsync(sale); 
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_Sale_Not_Found()
        {
            var saleId = 1;
            var saleRequest = new UpdateSaleCommand(saleId, new SaleDTOFake().Generate());

            Sale? sale = null;
            _saleRepository.GetByIdAsyncIncludes(Arg.Any<int>(), Arg.Any<Expression<Func<Sale, object>>[]>()).Returns(Task.FromResult(sale));

            var handler = new UpdateSaleHandler(_logger, _mapper, _discountFactory, _serviceProvider);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(saleRequest, CancellationToken.None));
            Assert.Equal("Venda não encontrada para o id.", exception.Message);
        }

        [Fact]
        public async Task Should_Apply_Discounts_Correctly()
        {
            var saleId = 1;
            var saleRequest = new UpdateSaleCommand(saleId, new SaleDTOFake().Generate());

            var sale = new Sale
            {
                Number = saleRequest.Sale.Number,
                Date = saleRequest.Sale.Date,
                Customer = saleRequest.Sale.Customer,
                Branch = saleRequest.Sale.Branch,
                Items = [.. saleRequest.Sale.Items.Select(i => new SaleItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalValue = i.Quantity * i.UnitPrice
                })]
            };

            var discountStrategy = Substitute.For<IDiscountStrategy>();
            discountStrategy.DiscountPercentage().Returns(10);

            _saleRepository.GetByIdAsyncIncludes(Arg.Any<int>(), Arg.Any<Expression<Func<Sale, object>>[]>()).Returns(Task.FromResult(sale));
            _saleItemRepository.DeleteRangeAsync(Arg.Any<List<SaleItem>>()).Returns(Task.CompletedTask);
            _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask);

            var handler = new UpdateSaleHandler(_logger, _mapper, _discountFactory, _serviceProvider);
            _discountFactory.CreateStrategy(Arg.Any<int>()).Returns(discountStrategy);

            _mapper.Map(Arg.Any<SaleDTO>(), Arg.Any<Sale>()).Returns(sale);

            await handler.Handle(saleRequest, CancellationToken.None);

            foreach (var item in sale.Items)
            {
                Assert.True(item.TotalValue > 0);
            }
        }
    }
}