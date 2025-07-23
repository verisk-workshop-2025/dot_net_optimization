using System.Text;
using System.Text.Json;
using DistributedSupermarket.Models;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("sales_exchange", ExchangeType.Topic, durable: true);

var sale = new SaleEvent(
    SaleId: 1001,
    StoreId: "NY-01",
    Items: [new("MILK001", 2), new("BREAD002", 1)],
    Timestamp: DateTime.UtcNow,
    TotalAmount: 9.49m
);

var message = JsonSerializer.Serialize(sale);
var body = Encoding.UTF8.GetBytes(message);

var properties = new BasicProperties();
await channel.BasicPublishAsync(
    exchange: "sales_exchange",
    routingKey: "sale.created",
    mandatory: true,
    basicProperties: properties,
    body: body
);

Console.WriteLine($"Sent SaleEvent #{sale.SaleId}.");
Console.WriteLine("Press [Enter] to exit.");
Console.ReadLine();


/*
    +-------------+         +-----------------+     ---> Analytics Queue
    | POS Service | ----->  | sales_exchange  |     ---> Inventory Queue
    +-------------+         +-----------------+     ---> Billing Queue                                                   

 */