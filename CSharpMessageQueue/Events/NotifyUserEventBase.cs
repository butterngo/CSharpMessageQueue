namespace CSharpMessageQueue.Events
{
    public abstract class NotifyUserEventBase
    {
        public NotifyUserEventBase() { }

        public NotifyUserEventBase(string connectionId, string uniqueKey)
        {
            ConnectionId = connectionId;
            UniqueKey = uniqueKey;
        }

        public string UniqueKey { get; set; }

        public string ConnectionId { get; set; }
    }
}
