using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static List<string> messages = new List<string>
    {
        "Hello!",
        "How are you doing?",
        "Let's chat!",
        "Here's a random message."
    };

    static Random rand = new Random();

    static void Main()
    {
        while (true)
        {
            try
            {
                string name = $"{rand.Next(1000, 9999)}";
                using TcpClient client = new TcpClient("127.0.0.1", 9000);
                Console.WriteLine($"Connected as {name}");

                var stream = client.GetStream();
                var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                var reader = new StreamReader(stream, Encoding.UTF8);

                writer.WriteLine(name);

                new Thread(() =>
                {
                    try
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine($"[Received] {line}");
                        }
                    }
                    catch { }
                }).Start();

                int msgCount = rand.Next(3, 7);
                for (int i = 0; i < msgCount; i++)
                {
                    string msg = messages[rand.Next(messages.Count)];
                    writer.WriteLine(msg);
                    Thread.Sleep(rand.Next(500, 1000));
                }

                client.Close();
                Console.WriteLine("Disconnected.\n");

                Thread.Sleep(rand.Next(1000, 2000));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                break;
            }
        }
    }
}