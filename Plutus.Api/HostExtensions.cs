using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plutus.Infrastructure;
using Serilog;

namespace Plutus.Api
{
    public static class HostExtensions
    {
        private static readonly object MigrationLock = new();
        
        public static IHost Migrate(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();
            try
            {
                lock (MigrationLock)
                {
                    context?.Database.Migrate();
                    Log.Information("Database Migration Completed Successfully");
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Database Migration Failed");
            }
            return host;
        }
    }
}