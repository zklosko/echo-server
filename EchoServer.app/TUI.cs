using Spectre.Console;
using EchoServer.State;
using System.ComponentModel;

namespace EchoServer.App
{
    internal class TUI
    {
        private readonly State.State _state;
        private int _selectedSpace = 1;
        private readonly Table _spacesTable;
        private readonly BarChart _zoneChart;
        private readonly Layout _layout;
        private readonly ManualResetEventSlim _stopSignal = new(false);
        private LiveDisplayContext? _ctx;
        private readonly object _renderLock = new();

        public TUI(State.State state, int port)
        {
            _state = state;

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

            _layout["Header"].Update(new Panel("[orange3]Echo Server[/]"));
            _layout["Spaces"].Update(new Panel(_spacesTable).Header("Spaces"));
            _layout["Zone"].Update(new Panel(_zoneChart).Header("Selected Zone"));
            _layout["Footer"].Update(new Text($"Listening on port {port}"));

        }

        public void Start()
        {
            AnsiConsole.Live(_layout).Start(ctx =>
            {
                _ctx = ctx;
                Refresh(0, State.State.ChangeType.Preset);
                _stopSignal.Wait();
            });
        }

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
                        _spacesTable.AddRow($"{i}", $"{preset}", isOff ? "Yes" : "No", seqSummary.Trim());
                    }

                    // get and display zone data
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
    }
}
