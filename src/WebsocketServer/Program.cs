using System;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

var socketServer = new WebSocketServer("ws://127.0.0.1:6666");
socketServer.AddWebSocketService<Echo>("/echo");
socketServer.Start();
Console.WriteLine("Server On");
var tokenSource = new CancellationTokenSource();
var token = tokenSource.Token;
while (!token.IsCancellationRequested)
{
    //check client connected
    if (socketServer.WebSocketServices.TryGetServiceHost("/echo", out WebSocketServiceHost host) && host.Sessions.Count > 0)
    {
        Stream inputStream = Console.OpenStandardInput();
        Memory<byte> bytes = new byte[100];
        await inputStream.ReadAsync(bytes);
        string s = Encoding.UTF8.GetString(bytes.Span);
        host.Sessions.BroadcastAsync(s, () => { });
    }
    await Task.Delay(1000);
}
socketServer.Stop();


public class Echo : WebSocketBehavior
{
    protected override void OnOpen()
    {
        Console.WriteLine("Open");
    }
    protected override void OnMessage(MessageEventArgs e)
    {
        Send(e.Data);
    }

}