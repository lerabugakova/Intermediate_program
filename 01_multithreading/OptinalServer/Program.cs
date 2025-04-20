using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static TcpListener listener;
    static List<TcpClient> clients = new List<TcpClient>();
    static ConcurrentQueue<string> messageHistory = new ConcurrentQueue<string>();
    const int MaxHistory = 50;
    static object clientLock = new object();

    static void Main()
    {
        listener = new TcpListener(IPAddress.Loopback, 9000);
        listener.Start();
        Console.WriteLine("Server started...");

        new Thread(ListenForClients).Start();

        Console.WriteLine("Press Enter to shut down server...");
        Console.ReadLine();
        ShutdownServer();
    }

    static void ListenForClients()
    {
        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            lock (clientLock) clients.Add(client);

            new Thread(() => HandleClient(client)).Start();
        }
    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        var reader = new StreamReader(stream, Encoding.UTF8);
        var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

        string name = reader.ReadLine();
        Console.WriteLine($"Client connected: {name}");

        foreach (var msg in messageHistory)
            writer.WriteLine(msg);

        try
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string fullMessage = $"[{name}] {line}";
                Console.WriteLine(fullMessage);
                AddToHistory(fullMessage);
                Broadcast(fullMessage, except: client);
            }
        }
        catch { }
        finally
        {
            Console.WriteLine($"Client disconnected: {name}");
            lock (clientLock) clients.Remove(client);
            client.Close();
        }
    }

    static void Broadcast(string message, TcpClient except = null)
    {
        lock (clientLock)
        {
            foreach (var c in clients)
            {
                if (c == except) continue;
                try
                {
                    var writer = new StreamWriter(c.GetStream(), Encoding.UTF8) { AutoFlush = true };
                    writer.WriteLine(message);
                }
                catch {  }
            }
        }
    }

    static void AddToHistory(string msg)
    {
        messageHistory.Enqueue(msg);
        while (messageHistory.Count > MaxHistory)
            messageHistory.TryDequeue(out _);
    }

    static void ShutdownServer()
    {
        string notice = "[Server] Shutting down...";
        Broadcast(notice);
        listener.Stop();

        lock (clientLock)
        {
            foreach (var c in clients) c.Close();
            clients.Clear();
        }

        Console.WriteLine("Server closed.");
    }
}