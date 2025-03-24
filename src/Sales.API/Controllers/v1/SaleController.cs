using Asp.Versioning;
using AutoMapper;
using Sales.API.Models;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.DTOs;
using MediatR;
using Sales.Application.Queries;
using Sales.Application.Commands;

namespace Sales.API.Controllers.v1
{
    /// <summary>
    /// Controller responsável pelas vendas
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mapper"></param>
    /// <param name="mediator"></param>
    [ApiVersion("1.0")]
    public class SaleController(ILogger<SaleController> logger, IMapper mapper, IMediator mediator) : BaseController(logger, mapper, mediator)
    {
        /// <summary>
        /// Busca o venda pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna os dados da venda especificada</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSaleById(int id)
        {
            _logger.LogInformation("Obter os dados da venda {id}", id);

            var sale = await _mediator.Send(new GetSaleByIdQuery(id));

            var response = _mapper.Map<SaleResponseModel>(sale);
            return Ok(response);
        }

        /// <summary>
        /// Busca todas as vendas
        /// </summary>
        /// <returns>Retorna todos as vendas</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string[]? orderBy = null, 
            [FromQuery] string? number = null, [FromQuery] string? customer = null, [FromQuery] string? branch = null, [FromQuery] bool? isCancelled = null,
            [FromQuery] decimal? totalValueMin = null, [FromQuery] decimal? totalValueMax = null)
        {
            _logger.LogInformation("Obter todas as vendas.");

            var sales = await _mediator.Send(new GetAllSalesQuery(page, pageSize, orderBy, number, customer, branch, isCancelled, totalValueMin, totalValueMax));

            var response = _mapper.Map<IEnumerable<SaleResponseModel>>(sales);
            return Ok(response);
        }

        /// <summary>
        /// Adiciona uma nova venda
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Retorna a venda criada</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddSale([FromBody] SaleRequestModel request)
        {
            _logger.LogInformation("Criar nova venda.");
            var saleRequest = _mapper.Map<SaleDTO>(request);

            var saleResponse = await _mediator.Send(new CreateSaleCommand(saleRequest));

            var response = _mapper.Map<SaleCreateResponseModel>(saleResponse);
            return CreatedAtAction(nameof(AddSale), response);
        }

        /// <summary>
        /// Atualiza uma venda
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] SaleRequestModel request)
        {
            _logger.LogInformation("Atualizar a venda {id}.", id);

            var saleRequest = _mapper.Map<SaleDTO>(request);

            await _mediator.Send(new UpdateSaleCommand(id, saleRequest));

            return NoContent();
        }

        /// <summary>
        /// Cancela uma venda
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Cancelar a venda {id}.", id);

            await _mediator.Send(new DeleteSaleCommand(id));
            
            return NoContent();
        }
    }
}