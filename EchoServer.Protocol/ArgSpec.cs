namespace EchoServer.Protocol;

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