using AutoMapper;
using Bogus;
using Sales.Application.DTOs;
using Sales.Application.Mapping;
using Sales.Domain.Entities;

namespace Sales.Tests.Application.Mapping
{
    public class MappingProfileTests
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            _configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configurationProvider.CreateMapper();
        }

        [Fact]
        public void Should_Map_SaleDTO_To_Sale()
        {
            var saleDto = new Faker<SaleDTO>()
                .RuleFor(s => s.Id, f => f.Random.Int(1, 100))
                .RuleFor(s => s.Number, f => f.Random.String(5))
                .RuleFor(s => s.Customer, f => f.Name.FullName())
                .RuleFor(s => s.Date, f => f.Date.Past())
                .Generate();

            var sale = _mapper.Map<Sale>(saleDto);

            Assert.NotNull(sale);
            Assert.Equal(saleDto.Id, sale.Id);
            Assert.Equal(saleDto.Number, sale.Number);
            Assert.Equal(saleDto.Customer, sale.Customer);
            Assert.Equal(saleDto.Date.Date, sale.Date.Date); // Compara as datas ignorando o horário
        }

        [Fact]
        public void Should_Map_Sale_To_SaleDTO()
        {
            var sale = new Faker<Sale>()
                .RuleFor(s => s.Id, f => f.Random.Int(1, 100))
                .RuleFor(s => s.Number, f => f.Random.String(5))
                .RuleFor(s => s.Customer, f => f.Name.FullName())
                .RuleFor(s => s.Date, f => f.Date.Past())
                .Generate();

            var saleDto = _mapper.Map<SaleDTO>(sale);

            Assert.NotNull(saleDto);
            Assert.Equal(sale.Id, saleDto.Id);
            Assert.Equal(sale.Number, saleDto.Number);
            Assert.Equal(sale.Customer, saleDto.Customer);
            Assert.Equal(sale.Date.Date, saleDto.Date.Date); // Compara as datas ignorando o horário
        }

        [Fact]
        public void Should_Map_SaleItemDTO_To_SaleItem()
        {
            var saleItemDto = new Faker<SaleItemDTO>()
                .RuleFor(s => s.ProductId, f => f.Random.Int(1, 100))
                .RuleFor(s => s.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(s => s.UnitPrice, f => f.Finance.Amount(1, 100))
                .Generate();

            var saleItem = _mapper.Map<SaleItem>(saleItemDto);

            Assert.NotNull(saleItem);
            Assert.Equal(saleItemDto.ProductId, saleItem.ProductId);
            Assert.Equal(saleItemDto.Quantity, saleItem.Quantity);
            Assert.Equal(saleItemDto.UnitPrice, saleItem.UnitPrice);
        }

        [Fact]
        public void Should_Map_SaleItem_To_SaleItemDTO()
        {
            var saleItem = new Faker<SaleItem>()
                .RuleFor(s => s.ProductId, f => f.Random.Int(1, 100))
                .RuleFor(s => s.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(s => s.UnitPrice, f => f.Finance.Amount(1, 100))
                .Generate();

            var saleItemDto = _mapper.Map<SaleItemDTO>(saleItem);

            Assert.NotNull(saleItemDto);
            Assert.Equal(saleItem.ProductId, saleItemDto.ProductId);
            Assert.Equal(saleItem.Quantity, saleItemDto.Quantity);
            Assert.Equal(saleItem.UnitPrice, saleItemDto.UnitPrice);
        }
    }
}