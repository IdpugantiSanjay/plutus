
namespace Plutus.Worker;

using Plutus.Application.Transactions.Commands;
using Plutus.Application.Transactions.Indexes;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { };
        factory.UserName = "guest";
        factory.Password = "guest";
        factory.VirtualHost = "/";
        factory.HostName = "localhost";

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare("transactionsExchange", ExchangeType.Fanout);
        channel.QueueDeclare(queue: "transactions", durable: true, exclusive: false, autoDelete: false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            var type = Convert.ToString(ea.BasicProperties.Headers["TYPE"]);
            Console.WriteLine(type);
            var body = ea.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);
            var transaction = JsonSerializer.Deserialize<CreateTransaction.Event>(body);

            var http = new HttpClient() { BaseAddress = new(Environment.GetEnvironmentVariable("SEARCH_API_HOST")!) };

            var httpResponse = await http.PostAsync
                ("/api/transactionIndex", JsonContent.Create(
                    new TransactionIndex.Index(
                        Guid.NewGuid(),
                        transaction.Username,
                        transaction.Category,
                        transaction.DateTime,
                        transaction.Amount,
                        transaction.Description,
                        transaction.IsCredit)
                    )
                );

            Console.WriteLine(httpResponse.StatusCode);
            Console.WriteLine(" [x] Received {0}", transaction);
        };
        channel.BasicConsume(queue: "transactions", autoAck: false, consumer: consumer);


        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            try
            {
                await Task.Delay(10_000, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }

    //public static void Run()
    //{
    //    var factory = new ConnectionFactory() { HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") };
    //    using var connection = factory.CreateConnection();
    //    using var channel = connection.CreateModel();

    //    channel.QueueDeclare(queue: "transactions", durable: true, exclusive: false, autoDelete: false);

    //    var consumer = new EventingBasicConsumer(channel);

    //    consumer.Received += (model, ea) =>
    //    {
    //        var body = ea.Body.ToArray();
    //        var message = Encoding.UTF8.GetString(body);
    //        Console.WriteLine(" [x] Received {0}", message);
    //    };

    //    channel.BasicConsume(queue: "hello",
    //                         autoAck: true,
    //                         consumer: consumer);
    //}
}
