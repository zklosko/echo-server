namespace EchoServer.State;

/// <summary>
/// Logging utility for development
/// </summary>
public static class Logger
{
    public static bool Verbose { get; set; } = false;

    /// <summary>
    /// Prints log to terminal if verbose flag is triggered
    /// </summary>
    /// <param name="message"></param>
    public static void Log(string message)
    {
        if (Verbose)
        {
            Console.WriteLine(message);
        }
    }

    /// <summary>
    /// Transforms EOM characters into printable strings in logs
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string Escape(string s)
    {
        return s.Replace("\r", "\\r").Replace("\n", "\\n");
    }
}