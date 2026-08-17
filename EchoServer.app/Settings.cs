using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace EchoServer.App;

public class Settings
{
    public int Port { get; set; } = 4703;
    public string Eom { get; set; } = "\r";
    public List<SubscriberSettings> Subscribers { get; set; } = new();

    /// <summary>
    /// Loads EOM, port, and subscribers from yaml file
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Settings Load(string path)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        
        string yamlText = File.ReadAllText(path);
        return deserializer.Deserialize<Settings>(yamlText);
    }
}
public class SubscriberSettings
{
    public string Ip { get; set; } = "";
    public int Port { get; set; }
}