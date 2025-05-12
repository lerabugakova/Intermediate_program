var manager = new RabbitMqServiceManager();

while (true)
{
    manager.StartPublisher();
    Console.WriteLine("Publisher is started.");

    Console.ReadLine();

    var status = await manager.CheckRabbitMqStatus("data");
    Console.WriteLine(status);

    Console.ReadLine();

    manager.StartConsumer();
    Console.WriteLine("Consumer is started.");

    Console.ReadLine();

    status = await manager.CheckRabbitMqStatus("data");
    Console.WriteLine(status);

    Console.ReadLine();

    manager.StopAll();
    Console.WriteLine("Publisher and Consumer are stopped.");

    status = await manager.CheckRabbitMqStatus("data");
    Console.WriteLine(status);

    Console.ReadLine();
}