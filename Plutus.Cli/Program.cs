// See https://aka.ms/new-console-template for more information

using CommandLine;
using Plutus.Application.Transactions.Commands;
using Serilog;
using System.Net.Http.Json;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
    .WriteTo.File("input-logs.log", rollingInterval: RollingInterval.Month, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();





EnsureEnvironment();

Log.Information("plutus {@Args}", string.Join(" ", args));

Parser.Default.ParseArguments<AddOptions>(args)
    .MapResult(
      (AddOptions opts) => RunAddAsync(opts).GetAwaiter().GetResult(),
      errs => 1);


static async Task<int> RunAddAsync(AddOptions options)
{
    var PLUTUS_SERVER_URL = Environment.GetEnvironmentVariable("PLUTUS_SERVER_URL")!;
    var PLUTUS_AUTH_TOKEN = Environment.GetEnvironmentVariable("PLUTUS_AUTH_TOKEN")!;

    var http = new HttpClient() { BaseAddress = new Uri(PLUTUS_SERVER_URL) };
    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", PLUTUS_AUTH_TOKEN);

    var response = await http.PostAsJsonAsync("/api/users/sanjay/transactions",
        new CreateTransaction.Request(options.Amount, options.DateTime, options.Description ?? "", Guid.Parse(options.Category), "sanjay", options.Credit ? Plutus.Domain.Enums.TransactionType.Income : Plutus.Domain.Enums.TransactionType.Expense));

    response.EnsureSuccessStatusCode();
    return 0;
}

static void EnsureEnvironment()
{
    var baseUrl = Environment.GetEnvironmentVariable("PLUTUS_SERVER_URL");
    var token = Environment.GetEnvironmentVariable("PLUTUS_AUTH_TOKEN");

    if (baseUrl is not { Length: > 0 })
        throw new ArgumentException("PLUTUS_SERVER_URL environment variable not set");

    if (token is not { Length: > 0 })
        throw new ArgumentException("PLUTUS_AUTH_TOKEN environment variable not set");
}