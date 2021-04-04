using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plutus.Api.Common;
using Plutus.Application.Exceptions;
using Plutus.Application.Users.Commands;
using Plutus.Domain.Exceptions;


namespace Plutus.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Get() => Ok(new {Status = "Success"});

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(Create.Request request)
        {
            try
            {
                return Ok(await _mediator.Send(request));
            }
            catch (InvalidUsernameException e)
            {
                return this.ErrorResult(e.Field, e.Message);
            }
            catch (InvalidEmailException e)
            {
                return this.ErrorResult(e.Field, e.Message);
            }
            catch (InvalidPasswordException e)
            {
                return this.ErrorResult(InvalidPasswordException.Field, e.Message);
            }
            catch (UsernameTakenException e)
            {
                return this.ErrorResult(e.Field, e.Message);
            }
            catch (EmailAlreadyExistsException e)
            {
                return this.ErrorResult(e.Field, e.Message);
            }
        }
    }
}