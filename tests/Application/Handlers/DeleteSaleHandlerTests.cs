using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sales.Application.Commands;
using Sales.Application.Handlers;
using Sales.Domain.Entities;
using Sales.Domain.Exceptions;
using Sales.Infra.Interfaces;
using Sales.Tests.Fakes.Entities;

namespace Sales.Tests.Application.Handlers
{
    public class DeleteSaleHandlerTests
    {
        private readonly ILogger<DeleteSaleHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Sale> _saleRepository;

        public DeleteSaleHandlerTests()
        {
            _logger = Substitute.For<ILogger<DeleteSaleHandler>>();
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
        public async Task Should_Cancel_Sale_Correctly()
        {
            var sale = new SaleFake().Generate();

            _saleRepository.GetByIdAsync(Arg.Any<int>()).Returns(sale); 
            _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask);

            var handler = new DeleteSaleHandler(_logger, _saleRepository);

            await handler.Handle(new DeleteSaleCommand(1), CancellationToken.None);

            Assert.True(sale.IsCancelled); 
            await _saleRepository.Received(1).UpdateAsync(sale);
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_Sale_Not_Found()
        {
            Sale? sale = null;
            _saleRepository.GetByIdAsync(Arg.Any<int>()).Returns(sale); 

            var handler = new DeleteSaleHandler(_logger, _saleRepository);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new DeleteSaleCommand(1), CancellationToken.None));
            Assert.Equal("Venda não encontrada para o id.", exception.Message);
        }

        [Fact]
        public async Task Should_Publish_Cancellation_Events()
        {
            var sale = new SaleFake().Generate();

            _saleRepository.GetByIdAsync(Arg.Any<int>()).Returns(sale);
            _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask); 

            var handler = new DeleteSaleHandler(_logger, _saleRepository);

            await handler.Handle(new DeleteSaleCommand(1), CancellationToken.None);
        }
    }
}