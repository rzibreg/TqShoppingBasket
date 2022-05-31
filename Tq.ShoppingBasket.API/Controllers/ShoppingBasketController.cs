using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Tq.ShoppingBasket.Application.Queries;

namespace Tq.ShoppingBasket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingBasketController : ControllerBase
    {
        private readonly ILogger<ShoppingBasketController> _logger;
        private readonly IMediator _mediator;

        public ShoppingBasketController(ILogger<ShoppingBasketController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Calculate basket total.
        /// </summary>
        /// <param name="basketQuery"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBasketTotal([FromBody] GetBasketTotalQuery basketQuery)
        {
            _logger.LogInformation("GetBasketTotal call");
            var total = await _mediator.Send(basketQuery);

            return Ok(total);
        }

        /// <summary>
        /// Returns all previous basket totals.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBasketTotalById()
        {
            _logger.LogInformation("GetBasketTotalById call");
            var result = await _mediator.Send(new GetAllTotalsQuery());

            return Ok(result);
        }
    }
}
