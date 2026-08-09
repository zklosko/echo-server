namespace EchoServer.State;

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