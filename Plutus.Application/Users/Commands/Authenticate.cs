using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Plutus.Application.Exceptions;
using Plutus.Application.Repositories;


namespace Plutus.Application.Users.Commands
{
    public static class Authenticate
    {
        public record Request(string Username, string Password) : IRequest<Response>;
        public record Response(string Token, string Username);
        
        public class Handler: IRequestHandler<Request, Response>
        {
            private readonly IConfiguration _configuration;
            private readonly IUserRepository _repository;

            public Handler(IConfiguration configuration, IUserRepository repository)
            {
                _configuration = configuration;
                _repository = repository;
            }
            
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var passwordHelper = new PasswordHelper();
                var user =  await _repository.FindByUsername(request.Username);

                if (user is null || !passwordHelper.ConfirmPassword(request.Password, request.Username, user.Password))
                    throw new UsernamePasswordMismatchException();
                
                var token = GenerateJwtAccessToken(request.Username);
                return new Response(token, request.Username);
            }
            
            
            private string GenerateJwtAccessToken(string username)
            {
                var secret = _configuration.GetSection("Jwt:Secret").Value;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(JwtRegisteredClaimNames.UniqueName, username)
                };  

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}