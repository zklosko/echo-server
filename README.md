# Echo Server

EchoServer is a drop-in software replacement for ETC Echo's UDP control interface for local plugin development and testing.

This project is still under active development. The future v1.0 release will include a finalized TUI, builds for Mac and Windows, and a full 1:1 compatible API.

## Getting Started

Head to releases to download the latest zip folder. Port, EOM character, and subscribers can all be configured with a `settings.yml` file in the same directory as the executable.

If no settings are specified, the default port is 4703 and the default EOM is `\r`.

A separate settings file location can be specified by calling `.\EchoServer.exe --file <filepath>` during launch.

### Example settings.yml

```yaml
port: 4703
eom: "\r"
subscribers:
  - ip: 127.0.0.1
    port: 5000
```
