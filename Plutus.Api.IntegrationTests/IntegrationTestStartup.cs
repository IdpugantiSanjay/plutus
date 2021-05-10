using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Plutus.Api.IntegrationTests
{
    public class IntegrationTestStartup : Startup
    {
        public IntegrationTestStartup(IConfiguration configuration) : base(configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
        }

        protected override void AddAuthentication(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(new AllowAnonymousFilter());
                options.Filters.Add(new FakeUserFilter());
            });
        }
    }
}