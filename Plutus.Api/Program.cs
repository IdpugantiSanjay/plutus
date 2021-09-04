namespace Plutus.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Plutus API Started");

        EnsureEnvironmentVariablesAreSet();
        CreateHostBuilder(args).Build().Migrate().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

    public static void EnsureEnvironmentVariablesAreSet()
    {
        var envVariables = new[] { "ASPNETCORE_ENVIRONMENT", /*"ELASTIC_HOST"*/ "PG_Connection" };

        foreach (var envVariable in envVariables)
            if (Environment.GetEnvironmentVariable(envVariable) is not { Length: > 0 }) throw new InvalidOperationException($"{envVariable} environment variable not set");
    }
}
