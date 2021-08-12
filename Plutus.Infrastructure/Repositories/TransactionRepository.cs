using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Plutus.Application.Repositories;
using Plutus.Application.Transactions.Queries;
using Plutus.Domain;


namespace Plutus.Infrastructure.Repositories;
public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Transaction>> FindAsync(FindTransactions.Request request)
    {
        var transactions = _context
            .Transactions
            .Where(t => !t.InActive)
            .Where(FilterByDate(request))
            .Where(FilterByDescription(request))
            .Where(FilterByCategory(request))
            .Include(t => t.Category)
            .ToListAsync();

        return transactions;
    }

    private static Expression<Func<Transaction, bool>> FilterByUsername(FindTransactions.Request request)
    {
        return t => t.Username == request.Username;
    }

    private static Expression<Func<Transaction, bool>> FilterByCategory(FindTransactions.Request request)
    {
        if (request.CategoryId != default)
            return t => t.CategoryId == request.CategoryId && t.TransactionType == request.TransactionType;
        return t => true;
    }

    private static Expression<Func<Transaction, bool>> FilterByDescription(FindTransactions.Request request)
    {
        if (request.Description is { Length: > 0 } description)
            return t => EF.Functions.Like(t.Description, $"%{description}%");
        return t => true;
    }

    private static Expression<Func<Transaction, bool>> FilterByDate(FindTransactions.Request request)
    {
        if (request.From != default && request.To == default)
            return (t) => t.Username == request.Username && t.DateTime >= request.From;

        if (request.From == default && request.To != default)
            return (t) => t.Username == request.Username && t.DateTime <= request.To;

        if (request.From != default && request.To != default)
            return (t) => t.Username == request.Username && t.DateTime >= request.From && t.DateTime <= request.To;

        return (t) => t.Username == request.Username;
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        var _ = await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        return await _context.Transactions.Include(np => np.Category).FirstAsync(t => t.Id == transaction.Id);
    }

    public Transaction Update(Transaction transaction) => _context.Transactions.Update(transaction).Entity;

    public async Task<Transaction?> FindByIdAsync(Guid id) =>
        await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
