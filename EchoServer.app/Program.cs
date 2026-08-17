using EchoServer.App;
using EchoServer.State;
using EchoServer.Transport;

// Select yaml file containing settings from command line args:
// app.exe --file <location>
static string SelectSettingsFile(string[] args)
{
    var fileArgPos = Array.IndexOf(args, "--file");
    if (fileArgPos == -1 || fileArgPos == args.Length - 1)
    {
        string exeDir = AppContext.BaseDirectory;
        return Path.Combine(exeDir, "settings.yml");
    }
    return args[fileArgPos + 1];
}

string path = SelectSettingsFile(args);
var serverSettings = Settings.Load(path);

var port = serverSettings.Port;
var eom = serverSettings.Eom;
var subscribers = serverSettings.Subscribers
    .Select(s => new Subscriber(s.Ip, s.Port))
    .ToList();
var state = new State(eom, subscribers);

var notifier = new SubscriberNotifier(state);
state.OnSpaceChanged = notifier.Notify;
    
var server = new UdpServer(port, state);
await server.RunAsync();