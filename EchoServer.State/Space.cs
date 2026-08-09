using System.Collections.Generic;
namespace EchoServer.State;

public class Space
{
    public int ActivePreset { get; set; }
    public Dictionary<int, int> Zones { get; set; }
    public Dictionary<int, int> Sequences { get; set; }

    public Space()
    {
        Zones = new Dictionary<int, int>();
        for (int i = 1; i <= 16; i++)
        {
            Zones[i] = 0;
        }
        Sequences = new Dictionary<int, int>();
        for (int s = 1; s <= 4; s++)
        {
            Sequences[s] = 0;
        }
        ActivePreset = 0;
    }
}