using DistributedSupermarket.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

int i = 0;
while (true)
{
    Console.Write("Press a to add new item and q to cancel/stop: ");
    ConsoleKeyInfo key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key == ConsoleKey.A)
    {
        await CreateEvent(i++);
    }
    else if (key.Key == ConsoleKey.Q)
    {
        break;
    }
}

static async Task CreateEvent(int eventId)
{
    ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };
    using IConnection connection = await factory.CreateConnectionAsync();
    using IChannel channel = await connection.CreateChannelAsync();

    await channel.ExchangeDeclareAsync("sales_exchange", ExchangeType.Topic, durable: true);

    SaleEvent sale = new SaleEvent(
        SaleId: 1000 + eventId,
        StoreId: "NY-01",
        Items: [new($"MILK00{eventId}", 5), new($"BREAD00{eventId}", 5)],
        Timestamp: DateTime.UtcNow,
        TotalAmount: 9.49m
    );

    string message = JsonSerializer.Serialize(sale);
    byte[] body = Encoding.UTF8.GetBytes(message);

    BasicProperties properties = new BasicProperties();
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
}


/*
    +-------------+         +-----------------+     ---> Analytics Queue
    | POS Service | ----->  | sales_exchange  |     ---> Inventory Queue
    +-------------+         +-----------------+     ---> Billing Queue                                                   

 */