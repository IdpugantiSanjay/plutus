using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plutus.Application.Transactions.Commands;
using Plutus.Application.Transactions.Queries;

namespace Plutus.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users/{username}/[controller]s")]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransaction.Request request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(request, cancellationToken));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] FindTransactionById.Request request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(request, cancellationToken));
        }

        [HttpGet]
        public async Task<IActionResult> FindAsync([FromQuery] FindTransactions.Request request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(request, cancellationToken));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateTransaction.Request request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(request, cancellationToken));
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] DeleteTransaction.Request request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(request, cancellationToken));
        }
    }
}