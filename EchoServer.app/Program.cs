using EchoServer.State;
using EchoServer.Transport;

var server = new UdpServer(4703, new State("\r"));
await server.RunAsync();