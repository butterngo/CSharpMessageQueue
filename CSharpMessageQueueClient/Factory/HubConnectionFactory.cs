namespace CSharpMessageQueueClient
{
    using CSharpMessageQueueClient.Events;
    using CSharpMessageQueueClient.Models;
    using Microsoft.AspNetCore.SignalR.Client;
    using System;
    using System.Threading.Tasks;

    internal class HubConnectionFactory
    {
        private static Lazy<HubConnection> _instance;

        public static HubConnection HubConnection => _instance.Value;

        public static void InitHubConnection
        (
            string url,
            Action<NotifyUserConnectEvent> notifyUserConnect,
            Action<NotifyUserDisConnectEvent> notifyUserDisconnect,
            Action<NotifyUserDuplicatedEvent> notifyUserDuplicated,
            Action<CSharpMessage> messageReceived,
            Action<CompletedMessageReceived> completedMessageReceived,
            Func<Exception, Task> onClosed)
        {
            _instance = new Lazy<HubConnection>(() =>
            {
                var hubConnection = new HubConnectionBuilder()
                                   .WithUrl(url, context => { }).Build();

                hubConnection.On("NotifyUserConnect", notifyUserConnect);

                hubConnection.On("NotifyUserDisconnect", notifyUserDisconnect);

                hubConnection.On("NotifyUserDuplicated", notifyUserDuplicated);

                hubConnection.On("MessageReceived", messageReceived);

                hubConnection.On("CompletedMessageReceived", completedMessageReceived);

                hubConnection.Closed += onClosed;

                return hubConnection;
            });
        }
    }
}
