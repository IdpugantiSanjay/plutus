using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;

namespace Plutus.Api.Common
{
    public static class ControllerBaseExtensions
    {
        public static IActionResult ErrorResult(this ControllerBase _, string field, string message)
        {
            return new ErrorResponse(field, message);
        }

        private class ErrorResponse : BadRequestObjectResult
        {
            public ErrorResponse(string field, string message) : base(400)
            {
                Value = new {Field = field, Message = message};
            }
        }
    }
}