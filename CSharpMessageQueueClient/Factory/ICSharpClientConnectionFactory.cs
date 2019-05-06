﻿namespace CSharpMessageQueueClient
{
    using CSharpMessageQueueClient.Events;
    using CSharpMessageQueueClient.Models;
    using System;
    using System.Threading.Tasks;

    public interface ICSharpClientConnectionFactory
    {
        event Action<CSharpMessage> OnHandleReceivedMessage;

        event Action<CompletedMessageReceivedEvent> OnHandleCompletedMessageReceived;

        event Action<CSharpMessage> OnHandlePublishFail;

        event Action<object, NotifyUserConnectEvent> OnNotifyUserConnect;

        event Action<object, NotifyUserDisConnectEvent> OnNotifyUserDisconnect;

        event Action<object, NotifyUserDuplicatedEvent> OnNotifyUserDuplicated;

        event Action<object, CSharpMessage> OnLostConnection;

        Task SendAsync(CSharpMessage message);

        Task ProcessCompletedMessageAsync(CSharpMessage message);

        void StartConnection();
    }
}
