using WebSocketSharp;
using WebSocketSharp.Server;

var wssv = new WebSocketServer("ws://127.0.0.1:6666");
wssv.AddWebSocketService<Echo>("/echo");
wssv.Start();
Console.WriteLine("Server On");
Console.ReadKey(true);
wssv.Stop();


public class Echo : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        Send(e.Data);
    }
}