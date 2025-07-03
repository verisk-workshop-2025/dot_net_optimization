using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Demo.DistributedProcessing.ConsoleDemo.Demos;

internal class DistributedProgramming
{
    public static async Task Run()
    {
        using CancellationTokenSource cts = new();
        Console.CancelKeyPress += (s, e) =>
        {
            Console.WriteLine("Cancellation requested...");
            cts.Cancel();
            e.Cancel = true;
        };

        try
        {
            Console.WriteLine("Press Ctrl-C to cancel/stop");
            ConnectionFactory factory = new()
            {
                HostName = "localhost",
                Port = 5672,
            };

            await using IConnection connection = await factory.CreateConnectionAsync();
            await using IChannel channel = await connection.CreateChannelAsync();

            await DeclareExchangeAndQueues(channel);

            // Consumer
            AsyncEventingBasicConsumer consumer = new(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                await ProcessMessageAsync(ea, channel);
            };
            await channel.BasicConsumeAsync(queue: "main_queue", autoAck: false, consumer: consumer);

            await Task.Delay(Timeout.Infinite, cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Shutdown triggered.");
        }
    }

    private static async Task DeclareExchangeAndQueues(IChannel channel)
    {
        // 1. Declare DLX and DLQ
        await channel.ExchangeDeclareAsync("dlx_exchange", ExchangeType.Direct, durable: true);
        await channel.QueueDeclareAsync("dlq",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        await channel.QueueBindAsync("dlq", "dlx_exchange", "dlq");

        // 2. Declare Retry Queue (has TTL + DLX → main)
        Dictionary<string, object?> retryArgs = new()
            {
                { "x-dead-letter-exchange", "main_exchange" },
                { "x-dead-letter-routing-key", "main" },
                { "x-message-ttl", 60_000 } // 60 seconds delay before retry
            };
        await channel.ExchangeDeclareAsync("retry_exchange", ExchangeType.Direct, durable: true);
        await channel.QueueDeclareAsync("retry_queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: retryArgs);
        await channel.QueueBindAsync("retry_queue", "retry_exchange", "retry");

        // 3. Declare Main Queue (with DLX → dlx_exchange)
        Dictionary<string, object?> mainArgs = new()
            {
                { "x-dead-letter-exchange", "dlx_exchange" },
                { "x-dead-letter-routing-key", "dlq" }
            };
        await channel.ExchangeDeclareAsync("main_exchange", ExchangeType.Direct, durable: true);
        await channel.QueueDeclareAsync("main_queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: mainArgs);
        await channel.QueueBindAsync("main_queue", "main_exchange", "main");
    }

    private static async Task ProcessMessageAsync(BasicDeliverEventArgs ea, IChannel channel)
    {
        byte[] body = ea.Body.ToArray();
        string message = Encoding.UTF8.GetString(body);

        Console.WriteLine($"{DateTime.Now}: Received: {message}");

        if (!message.Contains("fail"))
        {
            // message is processed successfully
            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            return;
        }

        // Simulate failure
        const int maxRetries = 3;
        int retryCount = 0;

        if (ea.BasicProperties.Headers != null &&
            ea.BasicProperties.Headers.TryGetValue("x-retry-count", out object? value) && value is not null)
        {
            retryCount = Convert.ToInt32(Encoding.UTF8.GetString((byte[])value));
        }

        if (retryCount >= maxRetries)
        {
            Console.WriteLine("Exceeded max retries. Sending to DLQ.");
            // should go to dlq
            await channel.BasicRejectAsync(ea.DeliveryTag, requeue: false);
            return;
        }

        int delay = (int)Math.Pow(2, retryCount) * 5_000; // e.g., exponential backoff
        Console.WriteLine($"With expiration: {delay}");

        BasicProperties properties = new()
        {
            Persistent = true,
            Headers = new Dictionary<string, object?>
            {
                ["x-retry-count"] = (retryCount + 1).ToString(),
            },
            Expiration = delay.ToString(),
        };

        // should go to retry queue then to main queue again
        await channel.BasicPublishAsync(
            exchange: "retry_exchange",
            routingKey: "retry",
            mandatory: true,
            basicProperties: properties,
            body: ea.Body);

        Console.WriteLine($"Retrying ({retryCount + 1})...");

        await channel.BasicAckAsync(ea.DeliveryTag, false); // Acknowledge original message
    }
}
