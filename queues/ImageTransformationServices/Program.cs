using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "data", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

while (true)
{
    Console.WriteLine(" [*] Waiting for messages.");
    var consumer = new AsyncEventingBasicConsumer(channel);
    consumer.ReceivedAsync += (model, ea) =>
    {
        byte[] body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($" [x] {message}");
        return Task.CompletedTask;
    };

    await channel.BasicConsumeAsync("data", autoAck: true, consumer: consumer);

    Console.ReadLine();
}