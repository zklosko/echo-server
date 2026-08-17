namespace EchoServer.Protocol;

/// <summary>
/// Arguments Spec type for validating the command schema
/// </summary>
public class ArgSpec
{
    public string Name { get; set; }
    public bool IsFloat { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }

    public ArgSpec(string name, bool isFloat, double min, double max)
    {
        Name = name;
        IsFloat = isFloat;
        Min = min;
        Max = max;
    }
}