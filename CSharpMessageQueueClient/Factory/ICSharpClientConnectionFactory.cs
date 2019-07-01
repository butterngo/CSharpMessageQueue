namespace CSharpMessageQueueClient
{
    using CSharpMessageQueueClient.Events;
    using CSharpMessageQueueClient.Models;
    using System;
    using System.Threading.Tasks;

    public interface ICSharpClientConnectionFactory
    {
        event Action<CSharpMessage> OnHandleReceivedMessage;

        event Action<CompletedMessageReceived> OnHandleCompletedMessageReceived;

        event Action<CSharpMessage> OnHandlePublishFail;

        event Action<object, NotifyUserConnectEvent> OnNotifyUserConnect;

        event Action<object, NotifyUserDisConnectEvent> OnNotifyUserDisconnect;

        event Action<object, NotifyUserDuplicatedEvent> OnNotifyUserDuplicated;

        event Action<object, CSharpMessage> OnLostConnection;

        Task ProcessCompletedMessageAsync(CSharpMessage message);

        Task SendAsync(CSharpMessage message);

        void StartConnection();
    }
}
