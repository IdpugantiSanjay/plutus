using System;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Plutus.Api.IntegrationTests
{
    public class IntegrationTestsApplicationFactory : WebApplicationFactory<IntegrationTestStartup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            return base.CreateHost(builder).Migrate();
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            Console.WriteLine("In CreateHostBuilder Method");
            var builder = Host
                    .CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(x =>
                    {
                        Console.WriteLine("In ConfigureWebHostDefaults Lambda");
                        x
                            .UseTestServer()
                            .ConfigureTestServices(s => { s.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>(); })
                            .UseStartup<IntegrationTestStartup>()
                            .UseSerilog()
                            ;
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddControllers().AddApplicationPart(typeof(Startup).Assembly);
                    })
                ;
            return builder;
        }
    }
}