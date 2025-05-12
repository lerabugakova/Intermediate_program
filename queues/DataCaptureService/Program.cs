using RabbitMQ.Client;
using System.Text;

namespace DataCaptureService
{
    class Program
    {
        const string filePath = "example.txt";
        static async Task Main(string[] args)
        {
            var number = 1;
            string content = $"Document Number {number++} .";

            File.WriteAllText(filePath, content);

            string messageBody = File.ReadAllText(filePath);

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "data", durable: false, exclusive: false, autoDelete: false,
                                             arguments: null);

            var body = Encoding.UTF8.GetBytes(messageBody);
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "data", body: body);
            Console.WriteLine($" [x] Sent {messageBody}");
        }
    }
}
