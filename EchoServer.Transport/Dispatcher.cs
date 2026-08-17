using EchoServer.State;

namespace EchoServer.Transport;

/// <summary>
/// Builds replies to send to subscribers and as ACK commands
/// </summary>
public class Dispatcher
{
    private static readonly string HELPTEXT = """
                                              E$pst act: spc_num(1-16), pst_num(1-64), time [EOM]
                                                  E$off: spc_num(1-16), time(0.0-25.4) [EOM]
                                                  E$seq act: spc_num(1-16), seq_num(1-4) [EOM]
                                                  E$seq dact: spc_num(1-16), seq_num(1-4) [EOM]
                                                  E$zone int: spc_num(1-16), zn_num(1-16), level(0-255), time(0.0-25.4) [EOM]
                                                  E$pst get: spc_num(1-16) [EOM]
                                                  E$off get: spc_num(1-16) [EOM]
                                                  E$seq get: spc_num(1-16) [EOM]
                                                  E$sync get: spc_num(0-16) [EOM]
                                                  E$zone int get: spc_num(1-16) [EOM]
                                                  E$help [EOM]
                                              """;

    public static string BuildSeqReply(int spaceNum, State.State state)
    {
        var msg = "";
        for (var s = 1; s <= 4; s++)
        {
            int status = state.GetSequenceStatus(spaceNum, s);
            msg += $"E>seq get: {spaceNum}, {s}, {status}{state.EOM}";
        }

        return msg;
    }
    
    public static string BuildZoneReply(int spaceNum, State.State state)
    {
        var msg = "";
        for (var z = 1; z <= 16; z++)
        {
            int level = state.GetZoneLevel(spaceNum, z);
            msg += $"E>zone int: {spaceNum}, {z}, {level}{state.EOM}";
        }

        return msg;
    }
    
    private static string BuildSyncReply(int spaceNum, State.State state)
    {
        var msg = $"E>LOK{state.EOM}";  // Begins with "sync ok" message
        var startSpace = spaceNum == 0 ? 1 : spaceNum;
        var endSpace = spaceNum ==  0 ? 16 : spaceNum;
        for (var space = startSpace; space <= endSpace; space++)
        {
            msg += BuildZoneReply(space, state);
            msg += BuildSeqReply(space, state);
            int preset = state.GetActivePreset(space);
            msg += $"E>pst get: {space}, {preset}{state.EOM}";
        }
        
        return msg;
    }
    
    /// <summary>
    /// Main reply-builder method
    /// </summary>
    /// <param name="verb">Command prefix</param>
    /// <param name="args"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string? Dispatch(string verb, double[] args, State.State state)
    {
        switch (verb)
        {
            // Fade engine not set up yet, discarding fade timings
            // SET
            case "pst act":
                state.SetActivePreset((int)args[0], (int)args[1]);
                return null;  // I'm pretty sure this actually returns something...
            case "off":
                state.SetSpaceOff((int)args[0]);
                return null;
            case "seq act":
                state.SetSequenceStatus((int)args[0], (int)args[1], true);
                return null;
            case "seq dact":
                state.SetSequenceStatus((int)args[0], (int)args[1], false);
                return null;
            case "zone int":
                state.SetZoneLevel((int)args[0], (int)args[1], (int)args[2]);
                return null;
            // GET
            case "pst get":
                int preset = state.GetActivePreset((int)args[0]);
                return $"E>pst get: {(int)args[0]}, {preset}{state.EOM}";
            case "off get":
                bool isOff = state.IsSpaceOff((int)args[0]);
                return $"E>off: {(int)args[0]}, {(isOff ? '1' : '0')}{state.EOM}";
            case "seq get":
                return BuildSeqReply((int)args[0], state);
            case "sync get":
                return BuildSyncReply((int)args[0], state);
            case "zone int get":
                return BuildZoneReply((int)args[0], state);
            case "help":
                return HELPTEXT;
            default:
                throw new ArgumentException($"no handler for verb: {verb}");
        }
    }
}