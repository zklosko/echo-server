namespace EchoServer.Protocol;

public static class CommandSchema
{
    public static readonly Dictionary<string, List<ArgSpec>> Table = new()
    {
        // SET actions
        ["pst act"] = new List<ArgSpec>
        {
            new ArgSpec("space", false, 1, 16),
            new ArgSpec("preset", false, 1, 64),
            new ArgSpec("fade_time", true, 0.0, 25.4),
        },
        ["off"] = new List<ArgSpec>
        {
            new ArgSpec("space", false, 1, 16),
            new ArgSpec("fade_time", true, 0.0, 25.4),
        },
        ["seq act"] = new List<ArgSpec>
        {
            new ArgSpec("space", false, 1, 16),
            new ArgSpec("seq", false, 1, 4),
        },
        ["seq dact"] = new List<ArgSpec>
        {
            new ArgSpec("space", false, 1, 16),
            new ArgSpec("seq", false, 1, 4), 
        },
        ["zone int"] = new List<ArgSpec>
        {
            new ArgSpec("space", false, 1, 16),
            new ArgSpec("zone", false, 1, 16),
            new ArgSpec("level", false, 0, 255),
            new ArgSpec("fade_time", true, 0.0, 25.4),
        },
        // GET actions
        ["pst get"] = new List<ArgSpec>
        {
            new ArgSpec("space", false, 1, 16),
        },
        ["off get"] = new List<ArgSpec>
        {
            new ArgSpec("space", false, 1, 16),
        },
        ["seq get"] = new List<ArgSpec>
        {
            new ArgSpec("space", false, 1, 16),
        },
        ["sync get"] = new List<ArgSpec>
        {
            new ArgSpec("space", false, 0, 16), // 0 = all spaces
        },
        ["zone int get"] = new List<ArgSpec>
        {
            new ArgSpec("space", false, 1, 16),
        },
        ["help"] = new List<ArgSpec>{}
    };
}