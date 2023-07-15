// See https://aka.ms/new-console-template for more information

using NetMQ;
using NetMQ.Monitoring;
using NetMQ.Sockets;

internal class Program
{
    public static void Main(string[] args)
    {
        string pubPort = Environment.GetEnvironmentVariable("PUB_PORT") ??
                          throw new InvalidOperationException("PUB_PORT not set");
        string subPort = Environment.GetEnvironmentVariable("SUB_PORT") ??
                          throw new InvalidOperationException("SUB_PORT not set");

        Console.WriteLine("Creating sockets");
        using XPublisherSocket pubSocket = new("@tcp://*:" + pubPort);
        using XSubscriberSocket subSocket = new("@tcp://*:" + subPort);

        Console.WriteLine("Creating monitors");
        using NetMQMonitor pubMonitor = new(pubSocket, "inproc://pub", SocketEvents.All);
        using NetMQMonitor subMonitor = new(subSocket, "inproc://sub", SocketEvents.All);

        pubMonitor.EventReceived += PubEventReceived;
        subMonitor.EventReceived += SubEventReceived;

        Console.WriteLine("Starting monitors");
        pubMonitor.StartAsync();
        subMonitor.StartAsync();

        Console.WriteLine("Starting proxy");
        Proxy proxy = new Proxy(subSocket, pubSocket);
        proxy.Start();
    }

    private static void PubEventReceived(object? sender, NetMQMonitorEventArgs e)
    {
        Console.WriteLine("[PUB] " + e.SocketEvent + " : " + e.Address);
    }

    private static void SubEventReceived(object? sender, NetMQMonitorEventArgs e)
    {
        Console.WriteLine("[SUB] " + e.SocketEvent + " : " + e.Address);
    }
}
