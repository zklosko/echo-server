using Spectre.Console;
using EchoServer.State;
using System.ComponentModel;

namespace EchoServer.App
{
    internal class TUI
    {
        private readonly State.State _state;
        private int _selectedSpace = 1;
        private readonly Text _instructions;
        private readonly Table _spacesTable;
        private readonly BarChart _zoneChart;
        private readonly Layout _layout;
        private readonly ManualResetEventSlim _stopSignal = new(false);
        private LiveDisplayContext? _ctx;
        private readonly object _renderLock = new();

        public TUI(State.State state, int port)
        {
            _state = state;

            _instructions = new Text("Use up and down keys to cycle between spaces");

            _spacesTable = new Table()
                .AddColumn("Space")
                .AddColumn("Preset")
                .AddColumn("Off?")
                .AddColumn("Sequences")
                .Expand();

            _zoneChart = new BarChart();

            _layout = new Layout("Root")
                .SplitRows(
                    new Layout("Header").Size(3),
                    new Layout("Body").Ratio(1).SplitColumns(
                        new Layout("Spaces"),
                        new Layout("Zone")
                        ),
                    new Layout("Footer").Size(3)
                );

            _layout["Header"].Update(new Panel(_instructions).Header("[orange3]Echo Server[/]").Expand());
            _layout["Spaces"].Update(new Panel(_spacesTable).Header("Spaces"));
            _layout["Zone"].Update(new Panel(_zoneChart));
            _layout["Footer"].Update(new Markup($"[CadetBlue_1]Listening on port {port}[/]"));

        }

        /// <summary>
        /// Starts TUI and refresh cycle
        /// </summary>
        public void Start()
        {
            _ = Task.Run(ListenForInput);

            AnsiConsole.Live(_layout).Start(ctx =>
            {
                _ctx = ctx;
                Refresh(0, State.State.ChangeType.Preset);
                _stopSignal.Wait();
            });
        }

        /// <summary>
        /// Draws TUI, each call refreshes display
        /// </summary>
        /// <param name="spaceNum"></param>
        /// <param name="changeType"></param>
        public void Refresh(int spaceNum, State.State.ChangeType changeType)
        {
            lock (_renderLock)
            {
                try
                {
                    // wipe table
                    _spacesTable.Rows.Clear();
                    _zoneChart.Data.Clear();

                    // get and display space data
                    for (int i = 1; i <= 16; i++)
                    {
                        int preset = _state.GetActivePreset(i);
                        bool isOff = _state.IsSpaceOff(i);
                        string seqSummary = "";
                        for (int s = 1; s <= 4; s++)
                        {
                            seqSummary += _state.GetSequenceStatus(i, s) + " ";
                        }
                        seqSummary = seqSummary.Trim();

                        if (i == _selectedSpace)
                        {
                            _spacesTable.AddRow($"[orange1]{i}[/]", $"[orange1]{preset}[/]", $"[orange1]{(isOff ? "Yes" : "No")}[/]", $"[orange1]{seqSummary}[/]");
                        }
                        else
                        {
                            _spacesTable.AddRow($"{i}", $"{preset}", isOff ? "Yes" : "No", seqSummary);

                        }
                    }

                    // get and display zone data
                    _zoneChart.Label("Zones in selected space");
                    for (int z = 1; z <= 16; z++)
                    {
                        _zoneChart.AddItem($"{z}", _state.GetZoneLevel(_selectedSpace, z));
                    }

                    _ctx?.Refresh();
                }
                catch (Exception e)
                {
                    Logger.Log($"TUI refresh failed: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Listens for keystrokes while program is running
        /// </summary>
        private void ListenForInput()
        {
            while (true)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    _selectedSpace = _selectedSpace == 1 ? 16 : _selectedSpace - 1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    _selectedSpace = _selectedSpace == 16 ? 1: _selectedSpace + 1;
                }

                Refresh(_selectedSpace, State.State.ChangeType.Zone);
            }
        }
    }
}
