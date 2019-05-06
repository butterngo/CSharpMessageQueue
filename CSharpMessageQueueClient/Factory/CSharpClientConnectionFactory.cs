namespace CSharpMessageQueueClient
{
    using System;
    using System.Threading.Tasks;
    using CSharpMessageQueueClient.Events;
    using CSharpMessageQueueClient.Models;
    using Microsoft.AspNetCore.SignalR.Client;
    using Polly;

    public class CSharpClientConnectionFactory : ICSharpClientConnectionFactory
    {
        public event Action<CSharpMessage> OnHandleReceivedMessage;

        public event Action<CompletedMessageReceivedEvent> OnHandleCompletedMessageReceived;

        public event Action<CSharpMessage> OnHandlePublishFail;

        public event Action<object, NotifyUserConnectEvent> OnNotifyUserConnect;

        public event Action<object, NotifyUserDisConnectEvent> OnNotifyUserDisconnect;

        public event Action<object, NotifyUserDuplicatedEvent> OnNotifyUserDuplicated;

        public event Action<object, CSharpMessage> OnLostConnection;

        public readonly HubConnection _hubConnection;

        private readonly object sync_root = new object();

        private bool IsConnected = false;

        public CSharpClientConnectionFactory(string host, string uniqueKey)
        {
            HubConnectionFactory.InitHubConnection($"{host}?uniqueKey={uniqueKey}",
                                                NotifyUserConnect,
                                                NotifyUserDisconnect,
                                                NotifyUserDuplicated,
                                                MessageReceived,
                                                CompletedMessageReceived,
                                                OnClosed);

            _hubConnection = HubConnectionFactory.HubConnection;

        }

        public async Task ProcessCompletedMessageAsync(CSharpMessage message)
        {
            await _hubConnection.SendAsync("ProcessCompletedMessageAsync", message);
        }

        public async Task SendAsync(CSharpMessage message)
        {
            if (IsConnected)
            {
                await _hubConnection.SendAsync("SendAsync", message);
            }
            else
            {
                if (OnLostConnection == null)
                {
                    throw new CSharpException("Lost connection to server.");
                }
                else
                {
                    OnLostConnection.Invoke(this, message);
                }
                
            }
            
        }

        public void StartConnection()
        {
            lock (sync_root)
            {
                var policy = Policy.Handle<CSharpException>()
                                   .WaitAndRetry(3,
                                                 retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                                 (ex, time) =>
                                                 {
                                                     //TODO
                                                 });

                policy.Execute(() =>
                {
                    _hubConnection.StartAsync()
                                  .GetAwaiter()
                                  .GetResult();
                });
            }
        }

        private Task OnClosed(Exception ex)
        {
            IsConnected = false;

            return Task.FromResult(0);
        }

        private void NotifyUserConnect(NotifyUserConnectEvent msg)
        {
            IsConnected = true;

            OnNotifyUserConnect?.Invoke(this, msg);

        }

        private void NotifyUserDisconnect(NotifyUserDisConnectEvent msg)
        {
            OnNotifyUserDisconnect?.Invoke(this, msg);
        }

        private void NotifyUserDuplicated(NotifyUserDuplicatedEvent msg)
        {
            if (OnNotifyUserDuplicated == null) throw new CSharpException($"UniqueKey: '{msg.UniqueKey}' is duplicated ");

            OnNotifyUserDuplicated?.Invoke(this, msg);
        }

        private void MessageReceived(CSharpMessage message)
        {
            OnHandleReceivedMessage?.Invoke(message);
        }

        private void CompletedMessageReceived(CompletedMessageReceivedEvent message)
        {
            OnHandleCompletedMessageReceived?.Invoke(message);
        }
    }
}
