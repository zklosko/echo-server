namespace EchoServer.State;

public static class Logger
{
    public static bool Verbose { get; set; } = false;

    public static void Log(string message)
    {
        if (Verbose)
        {
            Console.WriteLine(message);
        }
    }

    public static string Escape(string s)
    {
        return s.Replace("\r", "\\r").Replace("\n", "\\n");
    }
}