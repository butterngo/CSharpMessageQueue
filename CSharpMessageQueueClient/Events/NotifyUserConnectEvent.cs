namespace CSharpMessageQueueClient.Events
{
    public class NotifyUserConnectEvent : NotifyUserEventBase
    {
        public NotifyUserConnectEvent()
        {
        }

        public NotifyUserConnectEvent(string connectionId, string uniqueKey) : base(connectionId, uniqueKey)
        {
        }
    }
}
