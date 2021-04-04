using System;
using Microsoft.EntityFrameworkCore;
using Plutus.Infrastructure;

namespace Plutus.Application.UnitTests
{
    public class PlutusTestBase : IDisposable
    {
        protected readonly AppDbContext _context;

        protected PlutusTestBase()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();
        }
        
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}