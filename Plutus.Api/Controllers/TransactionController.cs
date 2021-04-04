using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plutus.Application.Transactions.Commands;
using Plutus.Application.Transactions.Queries;

namespace Plutus.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController: ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransaction.Request request, CancellationToken cancellationToken) 
            => Ok(await _mediator.Send(request, cancellationToken));

        [HttpGet]
        public async Task<IActionResult> FindAsync(FindTransactions.Request request, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken));
    }
}