namespace CSharpMessageQueue
{
    using CSharpMessageQueue.Events;
    using CSharpMessageQueue.Models;
    using log4net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class CSharpMessageHub: Hub
    {
        protected ILog Logger => LogManager.GetLogger(typeof(CSharpMessageHub));

        private readonly IConfiguration _configuration;

        private static ConcurrentDictionary<string, string> 
            _client = new ConcurrentDictionary<string, string>();

        private HttpContext context => Context.GetHttpContext();

        public CSharpMessageHub(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async override Task OnConnectedAsync()
        {
            var uniqueKey = context.Request.Query["uniqueKey"].ToString();

            if (_client.ContainsKey(uniqueKey))
            {
                await NotifyUserDuplicated(uniqueKey);
            }

            if (_client.TryAdd(uniqueKey, Context.ConnectionId))
            {
                WriteLog($"Connected: {uniqueKey}");

                await NotifyUserConnect(uniqueKey);
            }
            
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var uniqueKey = context.Request.Query["uniqueKey"].ToString();

            if (_client.TryRemove(uniqueKey, out string connectionId))
            {
                WriteLog($"DisConnected: {uniqueKey}");

                await NotifyUserDisConnect(uniqueKey);
            }
        }

        public async Task SendAsync(CSharpMessage message)
        {
            var body = Encoding.UTF8.GetString(message.Body);

            var logMsg = $"messageId: {message.Id.ToString()} To: {string.Join(",", message.Tos)} Content: {body}";

            WriteLog($"Server received message: {logMsg}");

            var listTask = new List<Task>();

            foreach (var to in message.Tos)
            {
                if (_client.TryGetValue(to, out string connectionId))
                {
                    listTask.Add(HandleSendMessage(connectionId, message));

                    WriteLog($"Send to {to} message: {logMsg}");
                }
                else
                {
                    WriteLog($"Not found {to} message: {logMsg}");
                }
            }

            await Task.WhenAll(listTask);
        }

        public async Task CompletedMessageReceived(CompletedMessageReceived message)
        {
            if (_client.TryGetValue(message.From, out string connectionId))
            {
                await Clients.Client(connectionId).SendCoreAsync("CompletedMessageReceived", new[] { message });
            }
            
        }

        private async Task NotifyUserConnect(string uniqueKey)
        {
            await Clients.Client(Context.ConnectionId)
                .SendCoreAsync("NotifyUserConnect", 
                new[] { new NotifyUserConnectEvent(Context.ConnectionId, uniqueKey) });
        }

        private async Task NotifyUserDisConnect(string uniqueKey)
        {
            await Clients.Client(Context.ConnectionId)
                .SendCoreAsync("NotifyUserDisConnect",
                new[] { new NotifyUserDisConnectEvent(Context.ConnectionId, uniqueKey) });
        }

        private async Task NotifyUserDuplicated(string uniqueKey)
        {
            await Clients.Client(Context.ConnectionId)
               .SendCoreAsync("NotifyUserDuplicated",
               new[] { new NotifyUserDisConnectEvent(Context.ConnectionId, uniqueKey) });
        }

        private Task HandleSendMessage(string connectionId, CSharpMessage message)
        {
            return Clients.Client(connectionId).SendCoreAsync("MessageReceived",new[] { message });
        }

        private void WriteLog(string logMsg)
        {
            if (_configuration.GetValue<bool>("EnableLog"))
            {
                Logger.Debug(logMsg);
            }
        }
    }
}
