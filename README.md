# Echo Server

1:1 compatible API server for ETC Echo plugin development for A/V control systems.

## Getting Started

Head to releases to download the latest executable.

## TODO

Core functionality

- [x] Subscribers list — data model exists (State.Subscribers), but nothing populates or uses it yet
- [ ] Subscriber config loading — read IPs/ports from a file (JSON) or CLI args at startup (written, untested)
- [x] Push updates to subscribers — after any successful "set" command (pst act, off, seq act/dact, zone int), send the equivalent "get"-style reply to every subscriber, not just the original sender
- [x] EOM as real config — currently hardcoded in Program.cs (new State("\r")); consider making it a CLI flag or config value instead

Polish / robustness

- [ ] XML doc comments (///) on the public methods in State, CommandParser, and Dispatcher — good time to do this now that shapes are stable
- [x] Consistent exception types — a couple of range checks still use ArgumentException where ArgumentOutOfRangeException would match the convention set elsewhere (worth a final sweep)
- [x] Reusable log-escaping helper — the \r/\n escaping trick for debug logging, if you want it in more than one place
- [x] Remove/gate debug Console.WriteLines once things are stable, or swap for a real logging library later

Distribution

- [x] Self-contained single-file publish — `dotnet publish EchoServer.App -c Release -r win-x64 --self-contained -p:PublishSingleFile=true` for whichever OS(es) your plugin developers use
