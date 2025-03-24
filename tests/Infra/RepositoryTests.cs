using Sales.Infra.Context;
using Sales.InfraEstructure.Repositories;
using Sales.Tests.Fakes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sales.Domain.Entities;

namespace Sales.Tests.Infra
{
    public class RepositoryTests
    {
        private readonly ILogger<Repository<Sale>> _logger;

        public RepositoryTests()
        {
            _logger = Substitute.For<ILogger<Repository<Sale>>>();
        }

        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DatabaseTest")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllItems()
        {
            var context = GetInMemoryDbContext();
            var repository = new Repository<Sale>(context, _logger);

            var salesFake = new SaleFake().Generate(3);

            context.Sales.AddRange(salesFake);
            await context.SaveChangesAsync();

            var sales = await repository.GetAllAsync();

            foreach (var saleFake in salesFake)
                Assert.Contains(sales, o => o.Id == saleFake.Id);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewItem()
        {
            var context = GetInMemoryDbContext();
            var repository = new Repository<Sale>(context, _logger);

            var saleFake = new SaleFake().Generate();

            await repository.AddAsync(saleFake);
            var sales = await repository.GetAllAsync();

            Assert.Contains(sales, o => o.Id == saleFake.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectItem()
        {
            var context = GetInMemoryDbContext();
            var repository = new Repository<Sale>(context, _logger);

            var saleFake = new SaleFake().Generate();

            context.Sales.Add(saleFake);
            await context.SaveChangesAsync();

            var sale = await repository.GetByKeysAsync(saleFake.Id);

            Assert.NotNull(sale);
            Assert.Equal(sale.Id, sale.Id);
            Assert.Equal(sale.Number, sale.Number);
            Assert.Equal(sale.Customer, sale.Customer);
            Assert.Equal(sale.Items.Count, sale.Items.Count);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveItem()
        {
            var context = GetInMemoryDbContext();
            var repository = new Repository<Sale>(context, _logger);

            var saleFake = new SaleFake().Generate();

            context.Sales.Add(saleFake);
            await context.SaveChangesAsync();

            await repository.DeleteAsync(saleFake.Id);
            var sales = await repository.GetAllAsync();

            Assert.DoesNotContain(sales, o => o.Id == saleFake.Id);
        }
    }
}
