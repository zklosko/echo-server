namespace EchoServer.Protocol;

public static class CommandParser
{
    public static (string Verb, string rawArgsStr) SplitCommand(string cmd)
    {
        string[] parts = cmd.Split(":");
        string verb = parts[0].Trim();

        string rawArgsStr;
        if (parts.Length == 1)
        {
            rawArgsStr = "";
        } else
        {
            rawArgsStr = parts[1].Trim();
        }

        return (verb, rawArgsStr);
    }

    public static string[] SplitArgs(string rawArgsStr)
    {
        if (rawArgsStr == "")
        {
            return Array.Empty<string>();
        }

        string[] args = rawArgsStr.Split(',');
        return args.Select(a => a.Trim()).ToArray();
    }

    public static List<ArgSpec> ValidateAndGetSpec(string verb, string[] args)
    {
        if (!CommandSchema.Table.TryGetValue(verb, out var spec))
        {
            throw new ArgumentException($"{verb} is not a valid command");
        }
        if (args.Length != spec.Count)
        {
            throw new ArgumentException($"incorrect number of arguments for {verb}");
        }

        return spec;
    }

    public static double[] ParseArgs(string[] args, List<ArgSpec> spec)
    {
        double[] parsed = new double[args.Length];

        for (int i = 0; i < args.Length; i++)
        {
            if (!double.TryParse(args[i], out double val))
            {
                throw new ArgumentException($"argument {args[i]} is not a number");
            }
            if (val < spec[i].Min || val > spec[i].Max)
            {
                throw new ArgumentException($"{args[i]} is out of range");
            }
                
            parsed[i] = val;
        }

        return parsed;
    }

    /// <summary>
    /// Parses a raw wire command string into a verb and validated arguments
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    public static (string Verb, double[] Args) Parse(string cmd)
    {
        var (verb, rawArgs) = SplitCommand(cmd);
        var args = SplitArgs(rawArgs);
        var argSpec = ValidateAndGetSpec(verb, args);
        var cleanArgs = ParseArgs(args, argSpec);

        return (verb, cleanArgs);
    }
}
