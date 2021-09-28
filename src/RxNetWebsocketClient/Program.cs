using System.Reactive.Linq;
using WebSocketSharp;

string wsPath = "ws://127.0.0.1:6666/echo";
using WebSocket webSocket = new(wsPath);

//-------------------------------------------------------
// Create an observable by wrapping ws.OnMessage
//-------------------------------------------------------
var globalEventStream = Observable
    .Using(
        () => webSocket,
        ws => Observable.FromEventPattern<EventHandler<MessageEventArgs>, MessageEventArgs>
        (
            addHandler => ws.OnMessage += addHandler,
            removeHandler => ws.OnMessage -= removeHandler
        ));
//---------------------------------------------------------
// Subscribe to globalEventStream
//---------------------------------------------------------
IDisposable subscription = globalEventStream.Subscribe(ep =>
{
    Console.WriteLine("Event Received");
    Console.WriteLine(ep.EventArgs.Data);
});

webSocket.Connect();
Console.WriteLine("Connected!");
webSocket.Send("Hello");
Console.WriteLine("Sended!");
Console.ReadKey();
subscription.Dispose();
webSocket.Close();