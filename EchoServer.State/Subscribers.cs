namespace EchoServer.State;

/// <summary>
/// Represents a subscriber with an IP address and port
/// </summary>
public class Subscriber
{
    public string Ip { get; set; }
    public int Port { get; set; }

    public Subscriber(string ip, int port)
    {
        Ip = ip;
        Port = port;
    }
}