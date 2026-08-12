# Echo Server

1:1 compatible API server for ETC Echo plugin development for A/V control systems.

## Getting Started

Head to releases to download the latest executable. Port, EOM character, and subscribers can all be configured with a `settings.yml` file in the same directory as `EchoServer.exe`.

If no settings are specified, the default port is 4703 and the default EOM is `\r`.

A seperate settings file location can be specified by calling `.\EchoServer.exe --file <filepath>` in Windows Terminal or Powershell.

### Example settings.yml

```yaml
port: 4703
eom: "\r"
subscribers:
  - ip: 127.0.0.1
    port: 5000
```
