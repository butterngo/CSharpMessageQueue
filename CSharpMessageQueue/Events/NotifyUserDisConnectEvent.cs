namespace CSharpMessageQueue.Events
{
    public class NotifyUserDisConnectEvent : NotifyUserEventBase
    {
        public NotifyUserDisConnectEvent()
        {
        }

        public NotifyUserDisConnectEvent(string connectionId, string uniqueKey) : base(connectionId, uniqueKey)
        {
        }
    }
}
