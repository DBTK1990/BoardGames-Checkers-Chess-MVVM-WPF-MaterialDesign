using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Web.WebSockets;
using System.Net.WebSockets;
using System.Threading;

namespace Chess0.WebSocketService
{
    class MyWebSocket : WebSocketHandler
    {

        string port = "51489";
        string function = "websocket.ashx";

        WebSocket ws = new WebSocket($"ws://localhost:{port}");



    }
}
