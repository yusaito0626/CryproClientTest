using Crypto_Clients;
using Crypto_Trading;
using Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Utils;
using static System.Reflection.Metadata.BlobBuilder;

namespace Crypto_Linux
{
    internal class websocketServer
    {
        private readonly HttpListener _listener = new();
        private readonly List<WebSocket> _clients = new();

        public List<logEntry> logList = new List<logEntry>();
        public List<fillInfo> dataFillList = new List<fillInfo>();
        
        public int sendingLogs = 0;
        public int sendingFills = 0;

        public Action<string, Enums.logType> _addLog;

        Dictionary<string, string> sendingItem = new Dictionary<string, string>();
        JsonSerializerOptions js_option = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private websocketServer()
        {

        }

        public async Task StartAsync(CancellationToken token)
        {
            string host = Environment.GetEnvironmentVariable("WS_HOST") ?? "http://localhost:8080/";
            this.addLog("Host: " + host);
            host = host.Trim('\"');
            _listener.Prefixes.Add(host);
            _listener.Start();
            this.addLog("WebSocket server started on " + host);

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

            while(Interlocked.CompareExchange(ref this.sendingLogs,1,0) != 0)
            {

            }
            int PageSize = 10;
            int i = 0;
            string json = "";
            
            string msg;
            while (i < this.logList.Count)
            {
                List<logEntry> subList;
                if (i + PageSize >= this.logList.Count)
                {
                    subList = this.logList.GetRange(i, this.logList.Count - i);
                }
                else
                {
                    subList = this.logList.GetRange(i, PageSize);
                }
                json = JsonSerializer.Serialize(subList);
                sendingItem["data_type"] = "log";
                sendingItem["data"] = json;
                msg = JsonSerializer.Serialize(sendingItem, this.js_option);
                this.BroadcastAsync(msg);
                i += PageSize;
            }
            Volatile.Write(ref this.sendingLogs, 0);
            while (Interlocked.CompareExchange(ref this.sendingFills, 1, 0) != 0)
            {

            }
            i = 0;
            while (i < this.dataFillList.Count)
            {
                List<fillInfo> subList;
                if (i + PageSize >= this.dataFillList.Count)
                {
                    subList = this.dataFillList.GetRange(i, this.dataFillList.Count - i);
                }
                else
                {
                    subList = this.dataFillList.GetRange(i, PageSize);
                }
                json = JsonSerializer.Serialize(subList);
                sendingItem["data_type"] = "fill";
                sendingItem["data"] = json;
                msg = JsonSerializer.Serialize(sendingItem, this.js_option);
                this.BroadcastAsync(msg);
                i += PageSize;
            }
            Volatile.Write(ref this.sendingFills, 0);

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

        public async Task processFill(fillInfo fill)
        {
            while (Interlocked.CompareExchange(ref this.sendingFills, 1, 0) != 0)
            {

            }
            this.dataFillList.Add(fill);

            string json = JsonSerializer.Serialize<List<fillInfo>>([fill]);
            string msg;
            sendingItem["data_type"] = "fill";
            sendingItem["data"] = json;
            msg = JsonSerializer.Serialize(sendingItem, this.js_option);
            this.BroadcastAsync(msg);
            Volatile.Write(ref this.sendingFills, 0);
        }
        public async Task processLog(logEntry log)
        {
            while (Interlocked.CompareExchange(ref this.sendingLogs, 1, 0) != 0)
            {

            }
            this.logList.Add(log);

            string json = JsonSerializer.Serialize<List<logEntry>>([log]);
            string msg;
            sendingItem["data_type"] = "log";
            sendingItem["data"] = json;
            msg = JsonSerializer.Serialize(sendingItem, this.js_option);
            this.BroadcastAsync(msg);
            Volatile.Write(ref this.sendingLogs, 0);
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
