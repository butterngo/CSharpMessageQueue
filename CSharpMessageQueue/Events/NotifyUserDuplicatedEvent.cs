namespace CSharpMessageQueue.Events
{
    public class NotifyUserDuplicatedEvent : NotifyUserEventBase
    {
        public NotifyUserDuplicatedEvent()
        {
        }

        public NotifyUserDuplicatedEvent(string connectionId, string uniqueKey) : base(connectionId, uniqueKey)
        {
        }
    }
}
