using AutoMapper;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sales.Application.DTOs;
using Sales.Application.Handlers;
using Sales.Application.Queries;
using Sales.Domain.Entities;
using Sales.Domain.Exceptions;
using Sales.Infra.Interfaces;
using Sales.Tests.Fakes.Entities;
using System.Linq.Expressions;

namespace Sales.Tests.Application.Handlers
{
    public class GetAllSalesHandlerTests
    {

        private readonly ILogger<GetAllSalesHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISaleRepository _saleRepository;

        public GetAllSalesHandlerTests()
        {
            _logger = Substitute.For<ILogger<GetAllSalesHandler>>();
            _mapper = Substitute.For<IMapper>();
            _serviceProvider = Substitute.For<IServiceProvider>();
            _saleRepository = Substitute.For<ISaleRepository>();

            var serviceScope = Substitute.For<IServiceScope>();
            serviceScope.ServiceProvider.Returns(_serviceProvider);

            var scopeFactory = Substitute.For<IServiceScopeFactory>();
            scopeFactory.CreateScope().Returns(serviceScope);

            _serviceProvider.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);
            _serviceProvider.GetService(typeof(ISaleRepository)).Returns(_saleRepository);
        }

        [Fact]
        public async Task Should_Return_Sales_When_Found()
        {
            var sales = new SaleFake().Generate(5);
            var salesDto = sales.Select(s => new SaleDTO { Id = s.Id, Number = s.Number, Date = s.Date, Customer = s.Customer, Branch = s.Branch });

            _saleRepository.GetSalesAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string[]>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool?>(), Arg.Any<decimal?>(), Arg.Any<decimal?>(), Arg.Any<Expression<Func<Sale, object>>[]>())
                .Returns(Task.FromResult(sales.AsEnumerable()));

            _mapper.Map<IEnumerable<SaleDTO>>(sales).Returns(salesDto);

            var handler = new GetAllSalesHandler(_logger, _mapper, _saleRepository);
            var query = new GetAllSalesQuery(1, 1, ["-branch", "number"], null, null, null, null, null, null);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(sales.Count, result.Count());

            await _saleRepository.Received(1).GetSalesAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string[]>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool?>(), Arg.Any<decimal?>(), Arg.Any<decimal?>(), Arg.Any<Expression<Func<Sale, object>>[]>());
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_No_Sales_Found()
        {
            List<Sale> sales = new();
            _saleRepository.GetSalesAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string[]>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool?>(), Arg.Any<decimal?>(), Arg.Any<decimal?>(), Arg.Any<Expression<Func<Sale, object>>[]>()).Returns(sales);

            var handler = new GetAllSalesHandler(_logger, _mapper, _saleRepository);
            var query = new GetAllSalesQuery(1, 1, ["-branch", "number"], null, null, null, null, null, null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
            Assert.Equal("Vendas não encontradas.", exception.Message);

            await _saleRepository.Received(1).GetSalesAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string[]>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool?>(), Arg.Any<decimal?>(), Arg.Any<decimal?>(), Arg.Any<Expression<Func<Sale, object>>[]>());
        }
    }
}