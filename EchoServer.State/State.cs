namespace EchoServer.State;

public class State
{
    public Dictionary<int, Space> Spaces { get; set; }
    public string EOM { get; set; }
    public List<Subscriber> Subscribers { get; set; }
    public enum ChangeType { Preset, Off, Sequence, Zone }
    public Action<int, ChangeType>? OnSpaceChanged { get; set; }
    private readonly object _lock = new();

    public State(string eom, List<Subscriber> subscribers)
    {
        Spaces = new Dictionary<int, Space>();
        for (int i = 1; i <= 16; i++)
        {
            Spaces[i] = new Space();
        }
        Subscribers = subscribers;
        EOM = eom;
    }

    public int GetActivePreset(int spaceNum)
    {
        lock (_lock)
        {
            if (!Spaces.ContainsKey(spaceNum))
            {
                throw new ArgumentException($"space {spaceNum} not found");
            }
            return Spaces[spaceNum].ActivePreset;
        }
    }

    public void SetActivePreset(int spaceNum, int preset)
    {
        lock (_lock)
        {
            if (!Spaces.ContainsKey(spaceNum))
            {
                throw new ArgumentException($"space {spaceNum} not found");
            }
            if (preset < 1 || preset > 64)
            {
                throw new ArgumentOutOfRangeException(nameof(preset), $"preset number {preset} not in range 1-64");
            }
            Spaces[spaceNum].ActivePreset = preset;
        }
        OnSpaceChanged?.Invoke(spaceNum, ChangeType.Preset);
    }

    public int GetZoneLevel(int spaceNum, int zoneNum)
    {
        lock (_lock)
        {
            if (!Spaces.ContainsKey(spaceNum))
            {
                throw new ArgumentException($"space {spaceNum} not found");
            }
            Space s = Spaces[spaceNum];
            if (!s.Zones.ContainsKey(zoneNum))
            {
                throw new ArgumentException($"zone {zoneNum} not found");
            }
            return s.Zones[zoneNum];
        }
    }

    public void SetZoneLevel(int spaceNum, int zoneNum, int level)
    {
        lock (_lock)
        {
            if (!Spaces.ContainsKey(spaceNum))
            {
                throw new ArgumentException($"space {spaceNum} not found");
            }
            Space s = Spaces[spaceNum];
            if (!s.Zones.ContainsKey(zoneNum))
            {
                throw new ArgumentException($"zone {zoneNum} not found");
            }
            if (level < 0 || level > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(level), $"level {level} outside of acceptable range 0-255");
            }
            s.Zones[zoneNum] = level;
        }
        OnSpaceChanged?.Invoke(spaceNum, ChangeType.Zone);
    }

    public int GetSequenceStatus(int spaceNum, int seqNum)
    {
        lock (_lock)
        {
            if (!Spaces.ContainsKey(spaceNum))
            {
                throw new ArgumentException($"space {spaceNum} not found");
            }
            Space s = Spaces[spaceNum];
            if (!s.Sequences.ContainsKey(seqNum))
            {
                throw new ArgumentException($"sequence {seqNum} not found");
            }
            return s.Sequences[seqNum];
        }
    }

    public void SetSequenceStatus(int spaceNum, int seqNum, bool active)
    {
        lock (_lock)
        {
            if (!Spaces.ContainsKey(spaceNum))
            {
                throw new ArgumentException($"space {spaceNum} not found");
            }
            Space s = Spaces[spaceNum];
            if (!s.Sequences.ContainsKey(seqNum))
            {
                throw new ArgumentException($"sequence {seqNum} not found");
            }
            s.Sequences[seqNum] = active ? 1 : 0;
        }
        OnSpaceChanged?.Invoke(spaceNum, ChangeType.Sequence);
    }

    public bool IsSpaceOff(int spaceNum)
    {
        lock (_lock)
        {
            if (!Spaces.ContainsKey(spaceNum))
            {
                throw new ArgumentException($"space {spaceNum} not found");
            }
            Space s = Spaces[spaceNum];
            foreach (var level in s.Zones.Values)
            {
                // If any level is not 0, the space is not considered off
                if (level != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public void SetSpaceOff(int spaceNum)
    {
        lock (_lock)
        {
            if (!Spaces.ContainsKey(spaceNum))
            {
                throw new ArgumentException($"space {spaceNum} not found");
            }
            Space s = Spaces[spaceNum];
            foreach (var zoneNum in s.Zones.Keys.ToList())
            {
                s.Zones[zoneNum] = 0;
            }
        }
        OnSpaceChanged?.Invoke(spaceNum, ChangeType.Off);
    }
}