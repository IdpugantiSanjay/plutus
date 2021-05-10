using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plutus.Application.Exceptions;
using Plutus.Application.Users.Commands;

namespace Plutus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class UserController: ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate(Authenticate.Request request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _mediator.Send(request, cancellationToken));
            }
            catch (UsernamePasswordMismatchException e)
            {
                return new UnauthorizedObjectResult(new {e.Message});
            }
        }
    }
}