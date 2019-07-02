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
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class CSharpMessageHubBase<T>: Hub where T : CSharpMessageBase
    {
        protected abstract ILog Logger { get; }

        protected readonly IConfiguration _configuration;

        protected static ConcurrentDictionary<string, CSharpProfile>
            _client = new ConcurrentDictionary<string, CSharpProfile>();

        protected abstract string UniqueKey { get; }

        protected abstract string Name { get; }

        protected HttpContext context => Context.GetHttpContext();

        public CSharpMessageHubBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async override Task OnConnectedAsync()
        {
            if (_client.ContainsKey(UniqueKey))
            {
                await NotifyUserDuplicated(UniqueKey);
            }

            var profile = new CSharpProfile(UniqueKey, Context.ConnectionId, Name);

            if (_client.TryAdd(UniqueKey, profile))
            {
                WriteLog($"Connected: {UniqueKey}");

                await NotifyUserConnected(profile);
            }

        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var uniqueKey = context.Request.Query["uniqueKey"].ToString();

            if (_client.TryRemove(uniqueKey, out CSharpProfile profile))
            {
                WriteLog($"DisConnected: {uniqueKey}");

                await NotifyUserDisConnected(profile);
            }
        }

        public abstract Task SendAsync(T message);

        public async Task CompletedMessageReceived(CompletedMessageReceived message)
        {
            var listTask = new List<Task>();

            if (_client.TryGetValue(message.From, out CSharpProfile profile))
            {
                listTask.AddRange(
                    profile.ConnectionIds.Select(connectionId
                    => Clients.Client(connectionId).SendCoreAsync("CompletedMessageReceived", new[] { message })));
            }

            await Task.WhenAll(listTask);
        }

        protected virtual async Task NotifyUserConnected(CSharpProfile profile)
        {
            await Clients.Client(Context.ConnectionId)
                .SendCoreAsync("NotifyUserConnected",
                new[] { new NotifyUserConnectEvent(Context.ConnectionId, profile.Id, profile) });
        }

        protected virtual async Task NotifyUserDisConnected(CSharpProfile profile)
        {
            await Clients.Client(Context.ConnectionId)
                .SendCoreAsync("NotifyUserDisConnected",
                new[] { new NotifyUserDisConnectEvent(Context.ConnectionId, profile.Id, profile) });
        }

        protected virtual async Task NotifyUserDuplicated(string uniqueKey)
        {
            await Clients.Client(Context.ConnectionId)
               .SendCoreAsync("NotifyUserDuplicated",
               new[] { new NotifyUserDisConnectEvent(Context.ConnectionId, uniqueKey) });
        }

        protected virtual Task HandleSendMessage(string connectionId, T message)
        {
            return Clients.Client(connectionId).SendCoreAsync("MessageReceived", new[] { message });
        }

        protected virtual Task HandleSendMessage(T message)
        {
            return Clients.All.SendCoreAsync("MessageReceived", new[] { message });
        }

        protected void WriteLog(string logMsg)
        {
            if (_configuration.GetValue<bool>("EnableLog"))
            {
                Logger.Debug(logMsg);
            }
        }
    }
}
