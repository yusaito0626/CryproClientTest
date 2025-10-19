using Crypto_Trading;
using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Linux
{
    internal class websocketServer
    {
        private readonly HttpListener _listener = new();
        private readonly List<WebSocket> _clients = new();

        public Action<string, Enums.logType> _addLog;

        private websocketServer()
        {

        }

        public async Task StartAsync(CancellationToken token)
        {
            _listener.Prefixes.Add("http://*:8080/");
            _listener.Start();
            this.addLog("WebSocket server started on ws://0.0.0.0:8080");
            
            while (!token.IsCancellationRequested)
            {
                var context = await _listener.GetContextAsync();

                if (context.Request.IsWebSocketRequest)
                {
                    try
                    {

                        var wsContext = await context.AcceptWebSocketAsync(null);
                        var socket = wsContext.WebSocket;
                        _clients.Add(socket);
                        _ = HandleClient(socket, token);
                    }
                    catch (Exception ex)
                    {
                        this.addLog($"WebSocket Accept Error: {ex}");
                        context.Response.StatusCode = 500;
                        context.Response.Close();
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        private async Task HandleClient(WebSocket socket, CancellationToken token)
        {
            var buffer = new byte[4096];

            this.addLog("Client connected");

            while (socket.State == WebSocketState.Open && !token.IsCancellationRequested)
            {
                var result = await socket.ReceiveAsync(buffer, token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    this.addLog("Client disconnected");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", token);
                    _clients.Remove(socket);
                }

                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                if (message == "ping")
                {
                    await socket.SendAsync(
                        Encoding.UTF8.GetBytes("pong"),
                        WebSocketMessageType.Text,
                        true,
                        token
                    );
                }

            }
        }

        public async Task BroadcastAsync(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            var seg = new ArraySegment<byte>(data);

            foreach (var ws in _clients.ToList())
            {
                if (ws.State == WebSocketState.Open)
                    await ws.SendAsync(seg, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public void addLog(string line, logType logtype = logType.INFO)
        {
            this._addLog("[websocketServer]" + line, logtype);
        }

        private static websocketServer _instance;
        private static readonly object _lockObject = new object();

        public static websocketServer GetInstance()
        {
            lock (_lockObject)
            {
                if (_instance == null)
                {
                    _instance = new websocketServer();
                }
                return _instance;
            }
        }
    }
}
