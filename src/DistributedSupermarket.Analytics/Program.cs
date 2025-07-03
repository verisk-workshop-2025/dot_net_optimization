using System.Text;
using System.Text.Json;
using DistributedSupermarket.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("sales_exchange", ExchangeType.Topic, durable: true);
await channel.QueueDeclareAsync("analytics_queue", durable: true, exclusive: false, autoDelete: false);
await channel.QueueBindAsync("analytics_queue", "sales_exchange", "sale.*");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (s, e) =>
{
    var body = e.Body.ToArray();
    var json = Encoding.UTF8.GetString(body);
    var sale = JsonSerializer.Deserialize<SaleEvent>(json);

    Console.WriteLine($"Analytics captured Sale #{sale?.SaleId} for Store {sale?.StoreId}");
    await channel.BasicAckAsync(e.DeliveryTag, false);
};

await channel.BasicConsumeAsync("analytics_queue", autoAck: false, consumer);
Console.WriteLine("Analytics Service running. Press [Enter] to exit.");
Console.ReadLine();