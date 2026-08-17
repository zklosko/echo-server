using System.Collections.Generic;
namespace EchoServer.State;

/// <summary>
/// A basic space for ETC Echo.
/// Contains 16 zones, 4 sequences, and 1 preset.
/// </summary>
public class Space
{
    public int ActivePreset { get; set; }
    public Dictionary<int, int> Zones { get; set; }
    public Dictionary<int, int> Sequences { get; set; }

    /// <summary>
    /// Creates a new space and initializes it with base values
    /// </summary>
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