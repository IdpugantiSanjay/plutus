using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;

namespace Plutus.Api;



public class Program
{
    public static void Main(string[] args)
    {
        EnsureEnvironmentVariablesAreSet();

        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["Elastic:ServerUrl"]))
            {
                TemplateName = "plutus-api-logs",
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
            })
            .CreateLogger();

        Log.Information("Program Started");
        CreateHostBuilder(args).Build().Migrate().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

    public static void EnsureEnvironmentVariablesAreSet()
    {
        var envVariables = new[] { "ASPNETCORE_ENVIRONMENT", "ELASTIC_HOST", "PGPASSWORD", "PGUSER", "PGDATABASE", "PGHOST" };

        foreach (var envVariable in envVariables)
            if (envVariable is not { Length: > 0 }) throw new InvalidOperationException($"{envVariable} environment variable not set");
    }
}
