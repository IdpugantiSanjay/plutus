using Microsoft.AspNetCore.Mvc;
using Plutus.Application.Transactions.Indexes;

namespace Plutus.SearchApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class TransactionIndexController : ControllerBase
{
    private readonly TransactionIndex index;

    public TransactionIndexController(TransactionIndex index)
    {
        this.index = index;
    }

    [HttpPost]
    public async Task IndexTransactionAsync(TransactionIndex.Index transaction, CancellationToken token)
    {
        await index.IndexAsync(transaction, token);
    }

    [HttpGet]
    public void GetTransactions()
    {

    }
}
