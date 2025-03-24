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

namespace Sales.Tests.Application.Handlers
{
    public class CreateSaleHandlerTests
    {
        private readonly ILogger<CreateSaleHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDiscountFactory _discountFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Sale> _saleRepository;

        public CreateSaleHandlerTests()
        {
            _logger = Substitute.For<ILogger<CreateSaleHandler>>();
            _mapper = Substitute.For<IMapper>();
            _discountFactory = Substitute.For<IDiscountFactory>();
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
        public async Task Should_Create_Sale_Correctly()
        {
            var saleRequest = new CreateSaleCommand(new SaleDTOFake().Generate());

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

            _saleRepository.GetWhereAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Sale, bool>>>()).Returns([]); 

            _mapper.Map<Sale>(Arg.Any<SaleDTO>()).Returns(sale);
            _mapper.Map<SaleDTO>(Arg.Any<Sale>()).Returns(new SaleDTO()); 

            var discountStrategy = Substitute.For<IDiscountStrategy>();
            _discountFactory.CreateStrategy(Arg.Any<int>()).Returns(discountStrategy);

            var handler = new CreateSaleHandler(_logger, _mapper, _discountFactory, _serviceProvider);

            var result = await handler.Handle(saleRequest, CancellationToken.None);

            Assert.NotNull(result);
            await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>()); 
        }

        [Fact]
        public async Task Should_Throw_DuplicateEntryException_When_Sale_Exists()
        {
            var saleRequest = new CreateSaleCommand(new SaleDTOFake().Generate());

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

            _saleRepository.GetWhereAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Sale, bool>>>()).Returns([sale]);

            _mapper.Map<Sale>(Arg.Any<SaleDTO>()).Returns(sale);
            _discountFactory.CreateStrategy(Arg.Any<int>()).Returns(Substitute.For<IDiscountStrategy>());

            var handler = new CreateSaleHandler(_logger, _mapper, _discountFactory, _serviceProvider);

            var exception = await Assert.ThrowsAsync<DuplicateEntryException>(() => handler.Handle(saleRequest, CancellationToken.None));
            Assert.Equal("Venda já cadastrada", exception.Message); 
        }

        [Fact]
        public void Should_Apply_Discount_To_Sale_Items()
        {
            var saleRequest = new CreateSaleCommand(new SaleDTOFake().Generate());

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

            _saleRepository.GetWhereAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Sale, bool>>>()).Returns([]);

            var discountStrategy = Substitute.For<IDiscountStrategy>();
            _discountFactory.CreateStrategy(Arg.Any<int>()).Returns(discountStrategy);

            _mapper.Map<Sale>(Arg.Any<SaleDTO>()).Returns(sale);

            var handler = new CreateSaleHandler(_logger, _mapper, _discountFactory, _serviceProvider);

            var result = handler.Handle(saleRequest, CancellationToken.None);

            discountStrategy.Received(3).ApplyDiscount(Arg.Any<SaleItem>());
        }
    }
}