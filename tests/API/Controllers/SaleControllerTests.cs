using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using MediatR;
using Sales.API.Controllers.v1;
using Sales.Application.DTOs;
using Sales.API.Models;
using Sales.Application.Commands;
using Sales.Application.Queries;
using Sales.Tests.Fakes.DTO;

namespace Sales.Tests.API.Controllers
{
    public class SaleControllerTests
    {
        private readonly SaleController _controller;
        private readonly IMediator _mediator = Substitute.For<IMediator>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly ILogger<SaleController> _logger = Substitute.For<ILogger<SaleController>>();

        public SaleControllerTests()
        {
            _controller = new SaleController(_logger, _mapper, _mediator);
        }

        [Fact]
        public async Task GetSaleById_ReturnsOk_WhenSaleExists()
        {
            var saleDto = new SaleDTOFake().Generate();
            _mediator.Send(Arg.Any<GetSaleByIdQuery>()).Returns(saleDto);
            _mapper.Map<SaleResponseModel>(saleDto).Returns(new SaleResponseModel { Id = saleDto.Id });

            var result = await _controller.GetSaleById(saleDto.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<SaleResponseModel>(okResult.Value);
            Assert.Equal(saleDto.Id, response.Id);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithSalesList()
        {
            var salesList = new SaleDTOFake().Generate(5);
            _mediator.Send(Arg.Any<GetAllSalesQuery>()).Returns(salesList);
            _mapper.Map<IEnumerable<SaleResponseModel>>(salesList).Returns(salesList.Select(s => new SaleResponseModel { Id = s.Id }));

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<IEnumerable<SaleResponseModel>>(okResult.Value);
            Assert.Equal(5, response.Count());
        }

        [Fact]
        public async Task AddSale_ReturnsCreated_WhenSaleIsAdded()
        {
            var saleRequest = new SaleRequestModel();
            var saleDto = new SaleDTOFake().Generate();
            _mapper.Map<SaleDTO>(saleRequest).Returns(saleDto);
            _mediator.Send(Arg.Any<CreateSaleCommand>()).Returns(saleDto);
            _mapper.Map<SaleCreateResponseModel>(saleDto).Returns(new SaleCreateResponseModel { Id = saleDto.Id });

            var result = await _controller.AddSale(saleRequest);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<SaleCreateResponseModel>(createdResult.Value);
            Assert.Equal(saleDto.Id, response.Id);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            var saleRequest = new SaleRequestModel();
            var saleDto = new SaleDTOFake().Generate();
            _mapper.Map<SaleDTO>(saleRequest).Returns(saleDto);

            var result = await _controller.Update(saleDto.Id, saleRequest);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            var saleDto = new SaleDTOFake().Generate();
            var result = await _controller.Delete(saleDto.Id);

            Assert.IsType<NoContentResult>(result);
        }
    }
}