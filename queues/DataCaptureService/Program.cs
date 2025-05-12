using RabbitMQ.Client;
using System.Text;

namespace DataCaptureService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string filePath = "example.txt";
            string content = "This is a sample text file created by C#.";

            File.WriteAllText(filePath, content);

            string messageBody = File.ReadAllText(filePath);

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "logs", type: ExchangeType.Fanout);

            var body = Encoding.UTF8.GetBytes(messageBody);
            await channel.BasicPublishAsync(exchange: "logs", routingKey: string.Empty, body: body);
            Console.WriteLine($" [x] Sent {messageBody}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
