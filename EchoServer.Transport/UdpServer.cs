using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using EchoServer.Protocol;

namespace EchoServer.Transport;

public class UdpServer
{
    private readonly UdpClient _udpClient;
    private readonly State.State _state;

    public UdpServer(int port, State.State state)
    {
        _udpClient = new UdpClient(port);
        _state = state;
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            const int SIO_UDP_CONNRESET = -1744830452;
            _udpClient.Client.IOControl((IOControlCode)SIO_UDP_CONNRESET, new byte[] { 0 }, null);
        }
    }

    public async Task RunAsync()
    {
        Console.WriteLine("Listening...");

        while (true)
        {
            try
            {
                var result = await _udpClient.ReceiveAsync();
                string message = Encoding.ASCII.GetString(result.Buffer);
                
                if (!message.StartsWith("E$"))
                {
                    throw new ArgumentException($"Incorrect prefix on message received: {message}");
                }

                message = message.Substring(2);
                
                var (verb, args) = CommandParser.Parse(message);
                string? reply = Dispatcher.Dispatch(verb, args, _state);
                if (reply != null)
                {
                    byte[] replyBytes = Encoding.ASCII.GetBytes(reply);
                    Console.WriteLine($"Sending {reply.Replace("\r", "\\r").Replace("\n", "\\n")} to {result.RemoteEndPoint}");
                    await _udpClient.SendAsync(replyBytes, reply.Length, result.RemoteEndPoint);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error handling packet: {e.Message}");
            }
        }
    }
}