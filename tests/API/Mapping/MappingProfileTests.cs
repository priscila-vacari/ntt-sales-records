using AutoMapper;
using Bogus;
using Sales.API.Mapping;
using Sales.API.Models;
using Sales.Application.DTOs;

namespace Sales.Tests.API.Mapping
{
    public class MappingProfileTests
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IMapper _mapper;
        private readonly Faker<SaleRequestModel> _saleRequestModelFaker;
        private readonly Faker<SaleResponseModel> _saleResponseModelFaker;
        private readonly Faker<SaleCreateResponseModel> _saleCreateResponseModelFaker;
        private readonly Faker<SaleItemRequestModel> _saleItemRequestModelFaker;
        private readonly Faker<SaleItemResponseModel> _saleItemResponseModelFaker;

        public MappingProfileTests()
        {
            _configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configurationProvider.CreateMapper();

            _saleRequestModelFaker = new Faker<SaleRequestModel>()
                .RuleFor(x => x.Number, f => f.Commerce.Ean13())
                .RuleFor(x => x.Customer, f => f.Name.FullName())
                .RuleFor(x => x.Items, f => [new SaleItemRequestModel { ProductId = f.Random.Int(1, 100), Quantity = f.Random.Int(1, 10), UnitPrice = f.Finance.Amount() }]);

            _saleResponseModelFaker = new Faker<SaleResponseModel>()
                .RuleFor(x => x.Id, f => f.Random.Int())
                .RuleFor(x => x.Number, f => f.Commerce.Ean13())
                .RuleFor(x => x.Customer, f => f.Name.FullName())
                .RuleFor(x => x.TotalValue, f => f.Finance.Amount());

            _saleCreateResponseModelFaker = new Faker<SaleCreateResponseModel>()
                .RuleFor(x => x.Id, f => f.Random.Int());

            _saleItemRequestModelFaker = new Faker<SaleItemRequestModel>()
                .RuleFor(x => x.ProductId, f => f.Random.Int(1, 100))
                .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(x => x.UnitPrice, f => f.Finance.Amount());

            _saleItemResponseModelFaker = new Faker<SaleItemResponseModel>()
                .RuleFor(x => x.ProductId, f => f.Random.Int(1, 100))
                .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(x => x.UnitPrice, f => f.Finance.Amount());
        }

        [Fact]
        public void ShouldMapSaleRequestModelToSaleDTO()
        {
            var saleRequest = _saleRequestModelFaker.Generate();

            var saleDto = _mapper.Map<SaleDTO>(saleRequest);

            Assert.NotNull(saleDto);
            Assert.Equal(saleRequest.Number, saleDto.Number);
            Assert.Equal(saleRequest.Customer, saleDto.Customer);
            Assert.Equal(saleRequest.Items.Count, saleDto.Items.Count);
        }

        [Fact]
        public void ShouldMapSaleResponseModelToSaleDTO()
        {
            var saleResponse = _saleResponseModelFaker.Generate();

            var saleDto = _mapper.Map<SaleDTO>(saleResponse);

            Assert.NotNull(saleDto);
            Assert.Equal(saleResponse.Id, saleDto.Id);
            Assert.Equal(saleResponse.Number, saleDto.Number);
            Assert.Equal(saleResponse.Customer, saleDto.Customer);
            Assert.Equal(saleResponse.TotalValue, saleDto.TotalValue);
        }

        [Fact]
        public void ShouldMapSaleCreateResponseModelToSaleDTO()
        {
            var saleCreateResponse = _saleCreateResponseModelFaker.Generate();

            var saleDto = _mapper.Map<SaleDTO>(saleCreateResponse);

            Assert.NotNull(saleDto);
            Assert.Equal(saleCreateResponse.Id, saleDto.Id);
        }

        [Fact]
        public void ShouldMapSaleItemRequestModelToSaleItemDTO()
        {
            var saleItemRequest = _saleItemRequestModelFaker.Generate();

            var saleItemDto = _mapper.Map<SaleItemDTO>(saleItemRequest);

            Assert.NotNull(saleItemDto);
            Assert.Equal(saleItemRequest.ProductId, saleItemDto.ProductId);
            Assert.Equal(saleItemRequest.Quantity, saleItemDto.Quantity);
            Assert.Equal(saleItemRequest.UnitPrice, saleItemDto.UnitPrice);
        }

        [Fact]
        public void ShouldMapSaleItemResponseModelToSaleItemDTO()
        {
            var saleItemResponse = _saleItemResponseModelFaker.Generate();

            var saleItemDto = _mapper.Map<SaleItemDTO>(saleItemResponse);

            Assert.NotNull(saleItemDto);
            Assert.Equal(saleItemResponse.ProductId, saleItemDto.ProductId);
            Assert.Equal(saleItemResponse.Quantity, saleItemDto.Quantity);
            Assert.Equal(saleItemResponse.UnitPrice, saleItemDto.UnitPrice);
        }
    }
}
