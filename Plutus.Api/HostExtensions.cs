using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plutus.Infrastructure;
using Serilog;

namespace Plutus.Api
{
    public static class HostExtensions
    {
        public static IHost Migrate(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();
            try
            {
                context?.Database.Migrate();
            }
            catch (Exception e)
            {
                Log.Error(e, "Database Migration Failed");
            }
            return host;
        }
    }
}