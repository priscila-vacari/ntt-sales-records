using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Sales.API.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mapper"></param>
    /// <param name="mediator"></param>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]s")]
    public abstract class BaseController(ILogger<BaseController> logger, IMapper mapper, IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        public readonly ILogger<BaseController> _logger = logger;

        /// <summary>
        /// Mapper
        /// </summary>
        public readonly IMapper _mapper = mapper;

        /// <summary>
        /// Mediator
        /// </summary>
        public readonly IMediator _mediator = mediator;
    }
}
