using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Plutus.Application.Exceptions;
using Plutus.Application.Repositories;
using Plutus.Domain;
using Plutus.Domain.Exceptions;

namespace Plutus.Application.Users.Commands
{
    public static class Create
    {
        public record Request
        (string Username, string Firstname, string Lastname, string Email,
            string Password) : IRequest<Response>;


        public record Response(string Username);    


        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="InvalidEmailException"></exception>
            /// <exception cref="InvalidUsernameException"></exception>
            /// <exception cref="InvalidPasswordException"></exception>
            /// <exception cref="EmailAlreadyExistsException"></exception>
            /// <exception cref="UsernameTakenException"></exception>
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                User user = new(
                    request.Username, request.Email, request.Firstname, request.Lastname, request.Password
                );

                var exists = await _repository.EmailAlreadyExists(user.Email);
                if (exists)
                    throw new EmailAlreadyExistsException(request.Email);

                exists = await _repository.UsernameAlreadyExists(user.Username);
                if (exists)
                    throw new UsernameTakenException(request.Username);

                await _repository.AddAsync(user);
                await _repository.SaveChangesAsync();

                return new Response(request.Username);
            }
        }
    }
}