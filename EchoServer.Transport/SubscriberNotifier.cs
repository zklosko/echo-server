using System.Net;
using System.Net.Sockets;
using System.Text;
using EchoServer.State;

namespace EchoServer.Transport;

public class SubscriberNotifier
{
    private readonly UdpClient _udpClient;
    private readonly State.State _state;

    public SubscriberNotifier(State.State state)
    {
        _udpClient = new UdpClient();
        _state = state;
    }

    private async Task SendSafeAsync(byte[] bytes, IPEndPoint endPoint)
    {
        try
        {
            await _udpClient.SendAsync(bytes, bytes.Length, endPoint);
        }
        catch (Exception e)
        {
            Logger.Log($"Failed to notify subscriber {endPoint}: {e.Message}");
        }
    }

    public void Notify(int spaceNum, State.State.ChangeType changeType)
    {
        string message = changeType switch
        {
            State.State.ChangeType.Preset => $"E>pst get: {spaceNum}, {_state.GetActivePreset(spaceNum)}{_state.EOM}",
            State.State.ChangeType.Off => $"E>space off: {spaceNum}, {(_state.IsSpaceOff(spaceNum) ? '1' : '0')}{_state.EOM}",
            State.State.ChangeType.Sequence => Dispatcher.BuildSeqReply(spaceNum, _state),
            State.State.ChangeType.Zone => Dispatcher.BuildZoneReply(spaceNum, _state),
            _ => throw new ArgumentException($"Unknown change type: {changeType}")
        };

        byte[] replyBytes = Encoding.ASCII.GetBytes(message);
        foreach (var subscriber in _state.Subscribers)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(subscriber.Ip), subscriber.Port);
            Logger.Log($"Sending {Logger.Escape(message)} to {subscriber.Ip}");
            _ = SendSafeAsync(replyBytes, endpoint);
        }
    }
}